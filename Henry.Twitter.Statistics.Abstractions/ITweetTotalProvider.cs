﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Statistics.Abstractions
{
    public interface ITweetTotalProvider
    {
        Task<int> GetTotalAsync(CancellationToken cancellationToken);
    }
}
