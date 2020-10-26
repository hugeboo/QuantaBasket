using QuantaBasket.Core.Contracts;
using QuantaBasket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantaBasket.Trader
{
    internal sealed class Signal : IQuantSignal, ITradeSignal
    {
        private readonly SecurityId _securityId = new SecurityId();
        private SignalSide _side;
        private long _qtty;
        private decimal _price;
        private PriceType _priceType;
        private string _quantName;

        public string Id { get; private set; }

        public SignalStatus Status { get; private set; }

        public DateTime CreatedTime { get; private set; }

        public string ClassCode 
        {
            get => _securityId.ClassCode;
            set
            {
                if (Status == SignalStatus.New) _securityId.ClassCode = value;
                else throw new InvalidOperationException(MakeForbiddenMessage());
            }
        }

        public string SecCode
        {
            get => _securityId.SecurityCode;
            set
            {
                if (Status == SignalStatus.New) _securityId.SecurityCode = value;
                else throw new InvalidOperationException(MakeForbiddenMessage());
            }
        }

        public SignalSide Side 
        {
            get => _side;
            set
            {
                if (Status == SignalStatus.New) _side = value;
                else throw new InvalidOperationException(MakeForbiddenMessage());
            }
        }

        public long Qtty 
        {
            get => _qtty;
            set
            {
                if (Status == SignalStatus.New) _qtty = value;
                else throw new InvalidOperationException(MakeForbiddenMessage());
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (Status == SignalStatus.New) _price = value;
                else throw new InvalidOperationException(MakeForbiddenMessage());
            }
        }

        public PriceType PriceType
        {
            get => _priceType;
            set
            {
                if (Status == SignalStatus.New) _priceType = value;
                else throw new InvalidOperationException(MakeForbiddenMessage());
            }
        }

        public long ExecQtty { get; private set; }

        public decimal AvgPrice { get; private set; }

        public DateTime LastUpdateTime { get; private set; }

        public string QuantName => _quantName;

        public Signal(string id, string quantName)
        {
            Id = id;
            Status = SignalStatus.New;
            CreatedTime = DateTime.Now;
            _quantName = quantName;
        }

        public bool PrimaryValidate(out string errorMessage)
        {
            var lstErrors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(ClassCode)) lstErrors.Add("ClassCode must be filled");
            if (string.IsNullOrWhiteSpace(SecCode)) lstErrors.Add("SecCode must be filled");
            if (Qtty <= 0) lstErrors.Add("Qtty must be > 0");
            if (Price < 0m) lstErrors.Add(PriceType == PriceType.Market ? "Price must be = 0" : "Price must be > 0");
            if (Price == 0m && PriceType != PriceType.Market) lstErrors.Add("Price must be > 0");

            if (lstErrors.Count > 0)
            {
                errorMessage = string.Join(", ", lstErrors);
                return false;
            }
            else
            {
                errorMessage = null;
                return true;
            }
        }

        public override string ToString()
        {
            return $"{Id} {ClassCode} {SecCode} {Side} {Qtty}@{Price} {PriceType}";
        }

        public SignalDTO ToSignalDTO()
        {
            return new SignalDTO
            {
                Id = Id,
                CreatedTime = CreatedTime,
                ClassCode = ClassCode,
                SecCode = SecCode,
                Status = Status,
                Side = Side,
                Qtty = Qtty,
                Price = Price,
                PriceType = PriceType,
                ExecQtty = ExecQtty,
                AvgPrice = AvgPrice,
                LastUpdateTime = LastUpdateTime,
                QuantName = QuantName
            };
        }

        public static Signal FromSignalDTO(SignalDTO dto)
        {
            var signal = new Signal(dto.Id, dto.QuantName)
            {
                CreatedTime = dto.CreatedTime,
                ClassCode = dto.ClassCode,
                SecCode = dto.SecCode,
                Status = dto.Status,
                Side = dto.Side,
                Qtty = dto.Qtty,
                Price = dto.Price,
                PriceType = dto.PriceType,
                ExecQtty = dto.ExecQtty,
                AvgPrice = dto.AvgPrice,
                LastUpdateTime = dto.LastUpdateTime
            };

            if (!signal.PrimaryValidate(out string err))
            {
                throw new ArgumentException(err);
            }

            return signal;
        }

        private string MakeForbiddenMessage()
        {
            return $"Changing this field is prohibited by status ({Status})";
        }
    }
}
