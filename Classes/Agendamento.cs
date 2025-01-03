using SQLite;

namespace LavaRapidoMobile.Classes
{
    public class Agendamento
    {
        private Agendamento? agendamento;

        public Agendamento()
        {

        }

        public Agendamento(Agendamento agendamento)
        {
            if (agendamento == null) throw new ArgumentNullException(nameof(agendamento));

            Proprietario = agendamento.Proprietario;
            Telefone = agendamento.Telefone;
            TipoVeiculo = agendamento.TipoVeiculo;
            ModeloVeiculo = agendamento.ModeloVeiculo;
            PlacaVeiculo = agendamento.PlacaVeiculo;
            TipoServico = agendamento.TipoServico;
            Funcionario = agendamento.Funcionario;
            Data = agendamento.Data;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Proprietario { get; set; }
        public string Telefone { get; set; }
        public string TipoVeiculo { get; set; }
        public string ModeloVeiculo { get; set; }
        public string PlacaVeiculo { get; set; }
        public string TipoServico { get; set; }
        public string Funcionario { get; set; }
        public DateTime Data {  get; set; }
    }
}
