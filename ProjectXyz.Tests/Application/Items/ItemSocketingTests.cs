﻿using System;

using Moq;
using Xunit;

using ProjectXyz.Application.Core.Enchantments;
using ProjectXyz.Application.Core.Items;
using ProjectXyz.Application.Interface.Enchantments;
using ProjectXyz.Application.Interface.Items;
using ProjectXyz.Data.Core.Enchantments;
using ProjectXyz.Data.Core.Stats;
using ProjectXyz.Data.Interface.Items.Materials;
using ProjectXyz.Tests.Xunit.Categories;
using ProjectXyz.Tests.Application.Items.Mocks;
using ProjectXyz.Tests.Application.Enchantments.Mocks;

namespace ProjectXyz.Tests.Application.Items
{
    [ApplicationLayer]
    [Items]
    public class ItemSocketingTests
    {
        [Fact]
        public void AddToOpenSocketSumsWeight()
        {
            var context = new Mock<IItemContext>();
            context
                .Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var socketCandidate = new Mock<IItem>();
            socketCandidate
                .Setup(x => x.RequiredSockets)
                .Returns(1);
            socketCandidate
                .Setup(x => x.Weight)
                .Returns(100);
            socketCandidate
                .Setup(x => x.Enchantments)
                .Returns(ProjectXyz.Application.Core.Enchantments.EnchantmentCollection.Create());

            var socketableItemData = ProjectXyz.Data.Core.Items.Item.Create();
            socketableItemData.Stats.Set(Stat.Create(ItemStats.TotalSockets, 1));
            socketableItemData.Stats.Set(Stat.Create(ItemStats.Weight, 50));
            var socketableItem = ItemBuilder
                .Create()
                .WithMaterialFactory(new Mock<IMaterialFactory>().Object)
                .Build(context.Object, socketableItemData);

            Assert.True(
                socketableItem.Socket(socketCandidate.Object),
                "Expecting the socket operation to be successful.");
            Assert.Contains(socketCandidate.Object, socketableItem.SocketedItems);
            Assert.Equal(
                socketableItem.TotalSockets - socketCandidate.Object.RequiredSockets,
                socketableItem.OpenSockets);
            Assert.Equal(150, socketableItem.Weight);
        }

        [Fact]
        public void AddToOpenSocketAppliesEnchantments()
        {
            var context = new Mock<IItemContext>();
            context
                .Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var socketCandidateEnchantment = new MockEnchantmentBuilder()
                .WithStatId(ItemStats.Value)
                .WithValue(123456)
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .Build();

            var socketCandidate = new Mock<IItem>();
            socketCandidate
                .Setup(x => x.RequiredSockets)
                .Returns(1);
            socketCandidate
                .Setup(x => x.Enchantments)
                .Returns(ProjectXyz.Application.Core.Enchantments.EnchantmentCollection.Create(new IEnchantment[]
                { 
                    socketCandidateEnchantment 
                }));

            var socketableItemData = ProjectXyz.Data.Core.Items.Item.Create();
            socketableItemData.Stats.Set(Stat.Create(ItemStats.TotalSockets, 1));
            var socketableItem = ItemBuilder
                .Create()
                .WithMaterialFactory(new Mock<IMaterialFactory>().Object)
                .Build(context.Object, socketableItemData);

            Assert.True(
                socketableItem.Socket(socketCandidate.Object),
                "Expecting the socket operation to be successful.");
            Assert.Contains(socketCandidate.Object, socketableItem.SocketedItems);
            Assert.Contains(socketCandidateEnchantment, socketableItem.Enchantments);
            Assert.Equal(socketCandidateEnchantment.Value, socketableItem.Value);
        }

        [Fact]
        public void SocketBadSocketCandidate()
        {
            var context = new Mock<IItemContext>();
            context
                .Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var socketCandidate = new Mock<IItem>();
            socketCandidate
                .Setup(x => x.RequiredSockets)
                .Returns(0);
            socketCandidate
                .Setup(x => x.Enchantments)
                .Returns(ProjectXyz.Application.Core.Enchantments.EnchantmentCollection.Create());

            var socketableItemData = ProjectXyz.Data.Core.Items.Item.Create();
            socketableItemData.Stats.Set(Stat.Create(ItemStats.TotalSockets, 1));
            var socketableItem = ItemBuilder
                .Create()
                .WithMaterialFactory(new Mock<IMaterialFactory>().Object)
                .Build(context.Object, socketableItemData);

            Assert.False(
                socketableItem.Socket(socketCandidate.Object),
                "Expecting the socket operation to be successful.");
            Assert.DoesNotContain(socketCandidate.Object, socketableItem.SocketedItems);
        }

        [Fact]
        public void AddNoOpenSockets()
        {
            var context = new Mock<IItemContext>();
            context
                .Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var socketCandidate = new Mock<IItem>();
            socketCandidate
                .Setup(x => x.RequiredSockets)
                .Returns(1);
            socketCandidate
                .Setup(x => x.Enchantments)
                .Returns(ProjectXyz.Application.Core.Enchantments.EnchantmentCollection.Create());

            var socketableItemData = ProjectXyz.Data.Core.Items.Item.Create();
            socketableItemData.Stats.Set(Stat.Create(ItemStats.TotalSockets, 1));
            var socketableItem = ItemBuilder
                .Create()
                .WithMaterialFactory(new Mock<IMaterialFactory>().Object)
                .Build(context.Object, socketableItemData);

            Assert.True(
                socketableItem.Socket(socketCandidate.Object),
                "Expecting the socket operation to be successful.");
            Assert.Equal(0, socketableItem.OpenSockets);
            Assert.False(
                socketableItem.Socket(socketCandidate.Object),
                "Expecting the second socket operation to fail.");
        }

        [Fact]
        public void SocketEnchantmentsCanExpire()
        {
            var socketCandidateEnchantment = new MockEnchantmentBuilder()
                .WithStatId(ItemStats.Value)
                .WithValue(123456)
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .WithRemainingTime(TimeSpan.FromSeconds(5), TimeSpan.Zero)
                .Build();

            var socketCandidate = new MockItemBuilder()
                .WithEnchantments(socketCandidateEnchantment)
                .WithRequiredSockets(1)
                .Build();

            var socketableItemData = ProjectXyz.Data.Core.Items.Item.Create();
            socketableItemData.Stats.Set(Stat.Create(ItemStats.TotalSockets, 1));
            var socketableItem = ItemBuilder
                .Create()
                .WithMaterialFactory(new Mock<IMaterialFactory>().Object)
                .Build(
                    new MockItemContextBuilder().Build(),
                    socketableItemData);

            Assert.True(
                socketableItem.Socket(socketCandidate),
                "Expecting the socket operation to be successful.");
            Assert.Equal(socketCandidateEnchantment.Value, socketableItem.Value);

            socketableItem.UpdateElapsedTime(socketCandidateEnchantment.RemainingDuration);
            Assert.DoesNotContain(socketCandidateEnchantment, socketableItem.Enchantments);
            Assert.Equal(0, socketableItem.Value);
        }
    }
}
