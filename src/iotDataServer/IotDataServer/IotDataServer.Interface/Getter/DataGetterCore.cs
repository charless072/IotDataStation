﻿using System;
using System.Threading.Tasks;
using Iot.Common.ClassLogger;
using Iot.Common.DataModel;

namespace IotDataServer.Interface.Getter
{
    public abstract class DataGetterCore : IDataGetter
    {
        private static readonly ClassLogger Logger = ClassLogManager.GetCurrentClassLogger();

        protected volatile bool IsStarted = false;
        protected volatile bool ShouldStop = false;
        protected volatile bool IsTestMode = false;
        protected volatile bool IsConfigUpdated = false;
        protected string ConfigFilepath = "";
        protected SimpleSettings Settings = new SimpleSettings();


        public virtual void Initialize(string configFilepath, bool isTestMode, SimpleSettings settings)
        {
            IsTestMode = isTestMode;
            ConfigFilepath = configFilepath;
            Settings = settings ?? new SimpleSettings();
            IsConfigUpdated = true;
        }

        public virtual void UpdatedConfig()
        {
            IsConfigUpdated = true;
        }

        public virtual bool Start()
        {
            if (IsStarted)
            {
                return true;
            }
            IsStarted = true;

            Task.Factory.StartNew(DoWork);

            return true;
        }

        public virtual void Stop()
        {
            ShouldStop = true;
        }

        protected virtual void DoWork()
        {
            try
            {
                DoRun();
            }
            catch (Exception e)
            {
                Logger.Error(e, "DoRun:");
            }
            try
            {
                DoDone();
            }
            catch (Exception e)
            {
                Logger.Error(e, "DoDone:");
            }
            IsStarted = false;
            ShouldStop = false;
        }

        protected abstract void DoRun();

        protected virtual void DoDone()
        {
        }

        public virtual void Dispose()
        {
            Stop();
        }
    }
}