using System;

namespace CriarCartaoApi.Models
{
    public class Cartao
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string DataSolicita { get; set; }
    }
}