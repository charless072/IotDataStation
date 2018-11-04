﻿using System;
using System.Diagnostics;
using System.Threading;
using Iot.Common.ClassLogger;

namespace IotDataServer.Interface.Getter
{
    public abstract class PollingDataGetterBase : DataGetterCore
    {
        private static readonly ClassLogger Logger = ClassLogManager.GetCurrentClassLogger();
        private int _doWorkTickInterval = 200;

        protected override void DoRun()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            bool isFirstTick = true;
            try
            {
                while (!ShouldStop)
                {
                    if (IsConfigUpdated)
                    {
                        isFirstTick = true;
                    }
                    if (isFirstTick || stopwatch.ElapsedMilliseconds > _doWorkTickInterval)
                    {
                        stopwatch.Restart();
                        try
                        {
                            DoWorkTick(isFirstTick, IsTestMode);
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e, "DoWorkTick");
                        }

                        if (isFirstTick)
                        {
                            isFirstTick = false;
                            IsConfigUpdated = false;
                        }
                        
                    }
                    else
                    {
                        Thread.Sleep(200);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "DoRun:");
            }
            
        }

        protected abstract void DoWorkTick(bool isFirstTick, bool isTestMode);
    }
}