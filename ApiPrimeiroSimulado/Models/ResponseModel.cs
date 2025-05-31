namespace ApiPrimeiroSimulado.Models
{
    public class ResponseModel<T>
    {
        public T? dados { get; set; }
        public string mensagem { get; set; } = string.Empty;
        public bool status { get; set; } = true;

        public ResponseModel() { }

        public ResponseModel(bool status, string mensagem)
        {
            this.status = status;
            this.mensagem = mensagem;
        }

        public ResponseModel(bool status, string mensagem, T? dados)
        {
            this.status = status;
            this.mensagem = mensagem;
            this.dados = dados;
        }
    }
}
