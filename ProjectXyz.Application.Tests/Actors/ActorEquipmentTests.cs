﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Moq;
using Xunit;

using ProjectXyz.Data.Core.Stats;
using ProjectXyz.Data.Core.Enchantments;
using ProjectXyz.Application.Interface.Items;
using ProjectXyz.Application.Interface.Items.ExtensionMethods;
using ProjectXyz.Application.Interface.Actors;
using ProjectXyz.Application.Interface.Actors.ExtensionMethods;
using ProjectXyz.Application.Core.Enchantments;
using ProjectXyz.Application.Core.Actors;
using ProjectXyz.Application.Core.Actors.ExtensionMethods;
using ProjectXyz.Application.Core.Items;
using ProjectXyz.Application.Tests.Items.Mocks;
using ProjectXyz.Application.Tests.Enchantments.Mocks;
using ProjectXyz.Data.Tests.Actors.Mocks;
using ProjectXyz.Tests.Xunit.Categories;

namespace ProjectXyz.Application.Tests.Actors
{
    [ApplicationLayer]
    [Actors]
    public class ActorEquipmentTests
    {
        [Fact]
        public void Actor_EquipEnchantedItem_BoostsStat()
        {
            const double BASE_LIFE = 100;
            const double ADDITIONAL_LIFE = 50;

            var enchantment = new MockEnchantmentBuilder()
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .WithStatId(ActorStats.MaximumLife)
                .WithValue(ADDITIONAL_LIFE)
                .WithTrigger(EnchantmentTriggers.Equip)
                .Build();

            var enchantedItem = new MockItemBuilder()
                .WithEnchantments(enchantment)
                .WithEquippableSlots("Some slot")
                .WithDurability(1, 1)
                .Build();

            var data = new MockActorBuilder()
                .WithStats(Stat.Create(ActorStats.MaximumLife, BASE_LIFE))
                .WithStats(Stat.Create(ActorStats.CurrentLife, BASE_LIFE))
                .Build();
            var context = new Mock<IActorContext>();
            context.
                Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var actor = Actor.Create(
                context.Object,
                data);

            AssertEquipItem(
                actor,
                enchantedItem,
                () =>
                {
                    Assert.Equal(BASE_LIFE, actor.GetCurrentLife());
                    Assert.Equal(BASE_LIFE + ADDITIONAL_LIFE, actor.GetMaximumLife());
                });
        }

        [Fact]
        public void Actor_EquipHoldTrigger_TriggersNothing()
        {
            const double BASE_LIFE = 100;
            const double ADDITIONAL_LIFE = 50;

            var enchantment = new MockEnchantmentBuilder()
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .WithStatId(ActorStats.MaximumLife)
                .WithValue(ADDITIONAL_LIFE)
                .WithTrigger(EnchantmentTriggers.Hold)
                .Build();

            var enchantedItem = new MockItemBuilder()
                .WithEnchantments(enchantment)
                .WithEquippableSlots("Some slot")
                .WithDurability(1, 1)
                .Build();

            var data = new MockActorBuilder()
                .WithStats(Stat.Create(ActorStats.MaximumLife, BASE_LIFE))
                .WithStats(Stat.Create(ActorStats.CurrentLife, BASE_LIFE))
                .Build();
            var context = new Mock<IActorContext>();
            context.
                Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var actor = Actor.Create(
                context.Object,
                data);

            AssertEquipItem(
                actor,
                enchantedItem,
                () =>
                {
                    Assert.Equal(BASE_LIFE, actor.GetCurrentLife());
                    Assert.Equal(BASE_LIFE, actor.GetMaximumLife());
                });
        }

        [Fact]
        public void Actor_HoldHoldTrigger_BoostsStat()
        {
            const double BASE_LIFE = 100;
            const double ADDITIONAL_LIFE = 50;

            var enchantment = new MockEnchantmentBuilder()
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .WithStatId(ActorStats.MaximumLife)
                .WithValue(ADDITIONAL_LIFE)
                .WithTrigger(EnchantmentTriggers.Hold)
                .Build();

            var enchantedItem = new MockItemBuilder()
                .WithEnchantments(enchantment)
                .WithEquippableSlots("Some slot")
                .WithDurability(1, 1)
                .Build();

            var data = new MockActorBuilder()
                .WithStats(Stat.Create(ActorStats.MaximumLife, BASE_LIFE))
                .WithStats(Stat.Create(ActorStats.CurrentLife, BASE_LIFE))
                .Build();
            var context = new Mock<IActorContext>();
            context.
                Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var actor = Actor.Create(
                context.Object,
                data);

            actor.Inventory.Add(enchantedItem, 0);
            Assert.Equal(BASE_LIFE, actor.GetCurrentLife());
            Assert.Equal(BASE_LIFE + ADDITIONAL_LIFE, actor.GetMaximumLife());
        }

        [Fact]
        public void Actor_EquipHealItem_CappedAtMaxLife()
        {
            const double MAX_LIFE = 100;
            const double BASE_LIFE = 50;
            const double HEAL_AMOUNT = 1000;

            var enchantment = new MockEnchantmentBuilder()
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .WithStatId(ActorStats.CurrentLife)
                .WithValue(HEAL_AMOUNT)
                .WithTrigger(EnchantmentTriggers.Equip)
                .Build();

            var enchantedItem = new MockItemBuilder()
                .WithEnchantments(enchantment)
                .WithEquippableSlots("Some slot")
                .WithDurability(1, 1)
                .Build();

            var data = new MockActorBuilder()
                .WithStats(Stat.Create(ActorStats.MaximumLife, MAX_LIFE))
                .WithStats(Stat.Create(ActorStats.CurrentLife, BASE_LIFE))
                .Build();
            var context = new Mock<IActorContext>();
            context.
                Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var actor = Actor.Create(
                context.Object,
                data);

            AssertEquipItem(
                actor,
                enchantedItem,
                () =>
                {
                    Assert.Equal(MAX_LIFE, actor.GetCurrentLife());
                    Assert.Equal(MAX_LIFE, actor.GetMaximumLife());
                });
        }

        [Fact]
        public void Actor_EquippedItemChangesEnchantments_StatsAreRecalculated()
        {
            const double LIFE_BUFF = 100;
            const double BASE_LIFE = 50;

            var enchantment = new MockEnchantmentBuilder()
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .WithStatId(ActorStats.MaximumLife)
                .WithValue(LIFE_BUFF)
                .WithTrigger(EnchantmentTriggers.Equip)
                .Build();

            var itemToBeEnchanted = new MockItemBuilder()
                .WithEquippableSlots("Some slot")
                .WithDurability(1, 1)
                .Build();

            var data = new MockActorBuilder()
                .WithStats(Stat.Create(ActorStats.MaximumLife, BASE_LIFE))
                .WithStats(Stat.Create(ActorStats.CurrentLife, BASE_LIFE))
                .Build();
            var context = new Mock<IActorContext>();
            context.
                Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var actor = Actor.Create(
                context.Object,
                data);

            AssertEquipItem(
                actor,
                itemToBeEnchanted,
                () =>
                {
                    // Execute
                    itemToBeEnchanted.Enchant(enchantment);

                    // Assert
                    Assert.Equal(BASE_LIFE, actor.GetCurrentLife());
                    Assert.Equal(BASE_LIFE + LIFE_BUFF, actor.GetMaximumLife());
                });
        }

        [Fact]
        public void Actor_EquipBrokenItem_Fails()
        {
            var item = new MockItemBuilder()
                .WithEquippableSlots("Some slot")
                .WithDurability(100, 0)
                .Build();

            var data = new MockActorBuilder().Build();
            var context = new Mock<IActorContext>();
            context.
                Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var actor = Actor.Create(
                context.Object,
                data);

            AssertCannotEquipItem(actor, item, item.EquippableSlots.First());
        }

        [Fact]
        public void Actor_BreakingEquipment_RemovesEnchantment()
        {
            var durabilityEnchantment = new MockEnchantmentBuilder()
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .WithStatId(ItemStats.CurrentDurability)
                .WithValue(-100)
                .WithTrigger(EnchantmentTriggers.Item)
                .Build();

            var lifeEnchantment = new MockEnchantmentBuilder()
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .WithStatId(ActorStats.MaximumLife)
                .WithValue(100)
                .WithTrigger(EnchantmentTriggers.Equip)
                .Build();

            var enchantedItem = Item.Create(
                new MockItemContextBuilder().Build(),
                new Data.Tests.Items.Mocks.MockItemBuilder()
                    .WithStats(
                        Stat.Create(ItemStats.CurrentDurability, 50), 
                        Stat.Create(ItemStats.MaximumDurability, 50))
                    .WithEquippableSlots("Some Slot")
                    .Build());
            enchantedItem.Enchant(lifeEnchantment);

            var data = new MockActorBuilder().Build();
            var context = new Mock<IActorContext>();
            context.
                Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var actor = Actor.Create(
                context.Object,
                data);

            AssertEquipItem(
                actor, 
                enchantedItem,
                () => Assert.Equal(lifeEnchantment.Value, actor.GetMaximumLife()));
            enchantedItem.Enchant(durabilityEnchantment);

            Assert.Equal(0, actor.GetMaximumLife());
        }

        [Fact]
        public void Actor_BreakingEquipment_UnequipsItem()
        {
            var durabilityEnchantment = new MockEnchantmentBuilder()
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .WithStatId(ItemStats.CurrentDurability)
                .WithValue(-100)
                .WithTrigger(EnchantmentTriggers.Item)
                .Build();

            var item = Item.Create(
                new MockItemContextBuilder().Build(),
                new Data.Tests.Items.Mocks.MockItemBuilder()
                    .WithStats(
                        Stat.Create(ItemStats.CurrentDurability, 50),
                        Stat.Create(ItemStats.MaximumDurability, 50))
                    .WithEquippableSlots("Some Slot")
                    .Build());

            var data = new MockActorBuilder().Build();
            var context = new Mock<IActorContext>();
            context.
                Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var actor = Actor.Create(
                context.Object,
                data);

            AssertEquipItem(actor, item);
            item.Enchant(durabilityEnchantment);

            Assert.False(
                actor.Equipment.HasItemEquipped(item),
                "Expecting the item to be unequipped.");
            Assert.Equal(item, new List<IItem>(actor.Inventory)[0]);
        }

        [Fact]
        public void Actor_BlessEnchantment_RemovesCurse()
        {
            var curseEnchantment = new MockEnchantmentBuilder()
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .WithStatId(ActorStats.MaximumLife)
                .WithValue(-10)
                .WithTrigger(EnchantmentTriggers.Equip)
                .WithStatusType(EnchantmentStatuses.Curse)
                .Build();

            var cursedItem = new MockItemBuilder()
                .WithEnchantments(curseEnchantment)
                .WithEquippableSlots("Some slot")
                .WithDurability(1, 1)
                .Build();

            var blessEnchantment = new MockEnchantmentBuilder()
                .WithCalculationId(EnchantmentCalculationTypes.Value)
                .WithStatId(ActorStats.Bless)
                .WithValue(0)
                .WithTrigger(EnchantmentTriggers.Equip)
                .Build();

            var blessItem = new MockItemBuilder()
                .WithEnchantments(blessEnchantment)
                .WithEquippableSlots("Magical Potion Drinking Slot")
                .WithDurability(1, 1)
                .Build();

            var data = new MockActorBuilder()
                .WithStats(Stat.Create(ActorStats.MaximumLife, 100))
                .WithStats(Stat.Create(ActorStats.CurrentLife, 100))
                .Build();
            var context = new Mock<IActorContext>();
            context.
                Setup(x => x.EnchantmentCalculator)
                .Returns(EnchantmentCalculator.Create());

            var actor = Actor.Create(
                context.Object,
                data);

            AssertEquipItem(
                actor,
                cursedItem,
                () => Assert.Equal(90, actor.GetMaximumLife()));

            AssertEquipItem(
                actor,
                blessItem,
                () => Assert.Equal(100, actor.GetMaximumLife()));
        }

        public void AssertCannotEquipItem(IActor actor, IItem item, string slot)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(item != null);

            Assert.False(
                actor.HasItemEquipped(item),
                "The actor already has '" + item + "' equipped.");
            Assert.False(
                actor.CanEquip(item, slot),
                "Expecting to fail to equip '" + item + "' to '" + slot + "'.");
            Assert.False(
                actor.HasItemEquipped(item),
                "The actor should not have '" + item + "' equipped.");
        }

        public void AssertEquipItem(IActor actor, IItem item, Action validationCallback = null)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(item != null);

            AssertEquipItems(
                actor,
                new IItem[] { item },
                validationCallback);
        }

        public void AssertEquipItems(IActor actor, IEnumerable<IItem> items, Action validationCallback = null)
        {
            Contract.Requires<ArgumentNullException>(actor != null);
            Contract.Requires<ArgumentNullException>(items != null);

            foreach (var item in items)
            {
                var slot = item.EquippableSlots.First();
                Assert.NotNull(item);
                Assert.True(
                    actor.CanEquip(item, slot),
                    "Expecting to equip '" + item  + "' to '" + slot + "'.");
                actor.Equip(item, slot);
                Assert.True(
                    actor.HasItemEquipped(item),
                    "Expecting item to be equipped in one of [" + string.Join(", ", item.EquippableSlots.ToArray()) + "] slots.");
            }

            if (validationCallback != null)
            {
                validationCallback.Invoke();
            }
        }
    }
}
