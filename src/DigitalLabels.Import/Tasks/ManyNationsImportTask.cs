﻿using DigitalLabels.Core.DomainModels;
using DigitalLabels.Import.Infrastructure;
using Raven.Abstractions.Data;
using Raven.Client;
using Serilog;

namespace DigitalLabels.Import.Tasks
{
    public class ManyNationsImportTask : ITask
    {
        private readonly IImportFactory<ManyNationsLabel> manyNationsLabelImportFactory;
        private readonly IDocumentStore store;

        public ManyNationsImportTask(
            IDocumentStore store,
            IImportFactory<ManyNationsLabel> manyNationsLabelImportFactory)
        {
            this.store = store;
            this.manyNationsLabelImportFactory = manyNationsLabelImportFactory;
        }

        public void Execute()
        {
            using (Log.Logger.BeginTimedOperation($"{GetType().Name} starting", $"{GetType().Name}.Execute"))
            {
                using (var bulkInsert = store.BulkInsert(options: new BulkInsertOptions { OverwriteExisting = true }))
                {
                    foreach (var manyNationsLabel in manyNationsLabelImportFactory.Fetch())
                    {
                        bulkInsert.Store(manyNationsLabel);
                    }
                }
            }
        }
    }
}
