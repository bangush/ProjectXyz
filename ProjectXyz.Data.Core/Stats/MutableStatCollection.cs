﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

using ProjectXyz.Data.Interface.Stats;

namespace ProjectXyz.Data.Core.Stats
{
    public class MutableStatCollection<TStat> : StatCollection<TStat>, IMutableStatCollection<TStat> where TStat : IStat
    {
        #region Constructors
        protected MutableStatCollection()
            : this(new TStat[0])
        {
        }

        protected MutableStatCollection(IEnumerable<TStat> stats)
            : base(stats)
        {
            Contract.Requires<ArgumentNullException>(stats != null);
        }
        #endregion

        #region Properties
        new public TStat this[string id]
        {
            get { return Stats[id]; }
            set { Stats[id] = value; }
        }
        #endregion

        #region Methods
        public static IMutableStatCollection<TStat> Create()
        {
            Contract.Ensures(Contract.Result<IMutableStatCollection<TStat>>() != null);
            return new MutableStatCollection<TStat>();
        }

        public static IMutableStatCollection<TStat> Create(IEnumerable<TStat> stats)
        {
            Contract.Requires<ArgumentNullException>(stats != null);
            Contract.Ensures(Contract.Result<IMutableStatCollection<TStat>>() != null);
            return new MutableStatCollection<TStat>(stats);
        }

        public void Set(TStat stat)
        {
            this.Stats[stat.Id] = stat;
        }

        public void Add(TStat stat)
        {
            this.Stats.Add(stat.Id, stat);
        }

        public void AddRange(IEnumerable<TStat> stats)
        {
            foreach (var stat in stats)
            {
                this.Stats.Add(stat.Id, stat);
            }
        }

        public void Remove(string id)
        {
            this.Stats.Remove(id);
        }

        public void Remove(TStat stat)
        {
            this.Stats.Remove(stat.Id);
        }

        public void RemoveRange(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                this.Stats.Remove(id);
            }
        }

        public void RemoveRange(IEnumerable<TStat> stats)
        {
            foreach (var stat in stats)
            {
                this.Stats.Remove(stat.Id);
            }
        }

        public new System.Collections.IEnumerator GetEnumerator()
        {
            return base.GetEnumerator();
        }
        #endregion
    }
}
