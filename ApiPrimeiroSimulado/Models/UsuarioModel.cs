using System.ComponentModel.DataAnnotations;

namespace ApiPrimeiroSimulado.Models
{
    public class UsuarioModel
    {
        [Key]
        public int idUsuario { get; set; }
        public string nomeUsuario { get; set; }
        public string email {  get; set; }
        public string senha { get; set; }
        public string tipo { get; set; }

    }
}
