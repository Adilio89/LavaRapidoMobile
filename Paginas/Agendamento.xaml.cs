using LavaRapidoMobile.Classes;
namespace LavaRapidoMobile.Paginas;

public partial class Agendamento : ContentPage
{
	public Agendamento(Agendamento agendamento)
	{
		InitializeComponent();
        CarregaAgendamentos(agendamento);
	}

    //Novo Metodo
    public void CarregaAgendamentos(Agendamento agendamento)
    {
        TXTID.Text = agendamento.Id.ToString();
        TXTProprietario.Text = agendamento.TXTProprietario.ToString();
        TXTTelefone.Text = agendamento.TXTTelefone.ToString();
        TXTTipoVeiculo.Text = agendamento.TXTTipoVeiculo.ToString();
        TXTModeloVeiculo.Text = agendamento.TXTModeloVeiculo.ToString();
        TXTPlacaVeiculo.Text = agendamento.TXTPlacaVeiculo.ToString();
        TXTTipoServico.Text = agendamento.TXTTipoServico.ToString();
        TXTFuncionario.Text = agendamento.TXTFuncionario.ToString();
    }

    private void BTNSalvar_Clicked(object sender, EventArgs e)
    {

    }

    private void BTNCancelar_Clicked(object sender, EventArgs e)
    {

    }
}