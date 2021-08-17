using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Artigos.Models
{
    public class ModelViewArtigoEscritor
    {
        public int Id { get; set; }
        public int EscritorId { get; set; }
        public string Titulo { get; set; }
        public byte[] Capa { get; set; }
        public int Ativo { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
    }
}