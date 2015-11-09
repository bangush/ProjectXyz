﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectXyz.Api.Messaging.Interface;

namespace ProjectXyz.Api.Interface
{
    public interface IResponder
    {
        #region Methods
        void Respond<TResponse>(Guid requestId)
            where TResponse : IResponse;

        void Respond<TResponse>(Guid requestId, Action<TResponse> callback)
            where TResponse : IResponse;
        #endregion
    }
}
