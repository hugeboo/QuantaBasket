using Newtonsoft.Json;
using NLog;
using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using QuantaBasket.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Core.Quant
{
    public abstract class AQuant<TConf> : IQuant, IHaveConfiguration where TConf : AQuantConfigurationSingleton<TConf>, new()
    {
        private QuantStatus _status = QuantStatus.Idle;

        protected ILogger Logger { get; private set; }

        public abstract string QuantBaseName { get; }

        public string Name { get; private set; }

        public HashSet<SecurityId> Securities { get; private set; }

        public QuantStatus Status
        {
            get
            {
                if (_status == QuantStatus.Error)
                {
                    return QuantStatus.Error;
                }
                else if (!Configuration.Enabled)
                {
                    return QuantStatus.Disabled;
                }
                else
                {
                    return _status;
                }
            }
        }

        protected abstract TConf Configuration { get; }

        protected IBasketService BasketService { get; private set; }

        public AQuant()
        {
            var dsec = JsonConvert.DeserializeAnonymousType(Configuration.Securities, new[] { new { c = "", s = "" } });
            Name = QuantBaseName + "-" + string.Join(",", dsec.Select(d => d.s));
            Securities = dsec.Select(d => new SecurityId { ClassCode = d.c, SecurityCode = d.s }).ToHashSet();

            Logger = LogManager.GetLogger(Name);
        }

        public virtual void Dispose()
        {
            //...
        }

        public object GetConfiguration()
        {
            return Configuration;
        }

        public virtual void Init(IBasketService basketService)
        {
            BasketService = basketService;
            BasketService.RegisterCallback(MessageProcessor);
        }

        public void SaveConfiguration()
        {
            Configuration.Save();
        }

        protected void SetStatus(QuantStatus newStatus)
        {
            if (Status != QuantStatus.Disabled && Status != QuantStatus.Error)
            {
                if (newStatus != QuantStatus.Disabled)
                {
                    _status = newStatus;
                }
            }
        }

        protected virtual void OnL1QuotationsMessage(L1QuotationsMessage m)
        {
        }

        protected virtual void OnSignalChangedMessage(SignalChangedMessage m)
        {
        }

        protected virtual void OnTimerMessage(TimerMessage m)
        {
        }

        protected virtual void OnStartMessage(StartMessage m)
        {
        }

        protected virtual void OnStopMessage(StopMessage m)
        {
        }

        private void MessageProcessor(AMessage message)
        {
            if (Status == QuantStatus.Disabled) return;
            switch (message)
            {
                case L1QuotationsMessage m:
                    OnL1QuotationsMessage(m);
                    break;
                case SignalChangedMessage m:
                    OnSignalChangedMessage(m);
                    break;
                case TimerMessage m:
                    OnTimerMessage(m);
                    break;
                case StartMessage m:
                    OnStartMessage(m);
                    break;
                case StopMessage m:
                    OnStopMessage(m);
                    break;
                case null:
                    throw new NullReferenceException("message");
            }
        }
    }
}
