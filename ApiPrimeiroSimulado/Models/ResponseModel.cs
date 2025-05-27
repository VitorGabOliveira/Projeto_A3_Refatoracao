namespace ApiPrimeiroSimulado.Models
{
    public class ResponseModel<T>
    {
        public T? dados { get; set; }
        public string mensagem {  get; set; } = string.Empty;
        public bool status { get; set; } = true;
    }
}
