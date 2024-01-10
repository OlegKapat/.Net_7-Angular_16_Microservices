using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ordering.Core.Common;

namespace Ordering.Core.Entities
{
    public class Order : EntityBase
    {
        public string? UserName { get; set; } = string.Empty;
        public decimal? TotalPrice { get; set; } = decimal.Zero;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }= string.Empty;
        public string? EmailAddress { get; set; }= string.Empty;
        public string? AddressLine { get; set; }= string.Empty;
        public string? Country { get; set; }= string.Empty;
        public string? State { get; set; }= string.Empty;
        public string? ZipCode { get; set; }= string.Empty;
        public string? CardName { get; set; }= string.Empty;
        public string? CardNumber { get; set; }= string.Empty;
        public string? Expiration { get; set; }= string.Empty;
        public string? Cvv { get; set; }= string.Empty;
        public int? PaymentMethod { get; set; }= int.MinValue;
    }
}
