﻿using System.Linq;
using DigitalLabels.Core.DomainModels;
using DigitalLabels.Import.Infrastructure;
using Raven.Abstractions.Data;
using Raven.Client;
using Serilog;

namespace DigitalLabels.Import.Tasks
{
    public class StandingStrongTask : ITask
    {
        private readonly IImportFactory<StandingStrongLabel> standingStrongLabelImportFactory;
        private readonly IDocumentStore store;

        public StandingStrongTask(
            IDocumentStore store,
            IImportFactory<StandingStrongLabel> standingStrongLabelImportFactory)
        {
            this.store = store;
            this.standingStrongLabelImportFactory = standingStrongLabelImportFactory;
        }

        public void Execute()
        {
            using (Log.Logger.BeginTimedOperation($"{GetType().Name} starting", $"{GetType().Name}.Execute"))
            {                
                var standingStrongLabels = standingStrongLabelImportFactory.Fetch();

                using (var bulkInsert = store.BulkInsert(options: new BulkInsertOptions { OverwriteExisting = true }))
                {
                    foreach (var standingStrongLabel in standingStrongLabels)
                    {
                        bulkInsert.Store(standingStrongLabel);
                    }
                }
            }
        }
    }
}
