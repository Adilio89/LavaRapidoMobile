using LavaRapidoMobile.Classes;
using LavaRapidoMobile.Paginas;
using System.Collections.ObjectModel;
using Agendamento = LavaRapidoMobile.Classes.Agendamento;


namespace LavaRapidoMobile
{
    public partial class MainPage : TabbedPage
    {
        private bool deslizar = false;

        private readonly Servico _servicoConexao;
        public ObservableCollection<Agendamento> AgendamentosEmAndamento { get; set; }

        public MainPage()
        {
            InitializeComponent();
            _servicoConexao = new Servico();
            AgendamentosEmAndamento = new ObservableCollection<Agendamento>();
            CVLista.ItemsSource = AgendamentosEmAndamento;

            CarregaLista();
        }

        private async void CarregaLista()
        {
            try
            {
                var agendamentos = await _servicoConexao.GetTodosAgendamentos();
                AgendamentosEmAndamento.Clear();

                foreach (var agendamento in agendamentos)
                {
                    AgendamentosEmAndamento.Add(agendamento);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao carregar lista: {ex.Message}", "OK");
            }
        }

        private async void BTNSalvar_Clicked(object sender, EventArgs e)
        {
           
            // Criando objeto de agendamento
            var agendamento = new Agendamento
            {
                Proprietario = txtProprietario.Text,
                Telefone = txtTelefone.Text,
                TipoVeiculo = cbxVeiculo.SelectedItem?.ToString(),
                ModeloVeiculo = txtModelo.Text,
                PlacaVeiculo = txtPlaca.Text,
                TipoServico = cbxServico.SelectedItem?.ToString(),
                Funcionario = txtFuncionario.Text,
                Data = DateTime.Now
            };

            if (string.IsNullOrWhiteSpace(agendamento.Proprietario) ||
               string.IsNullOrWhiteSpace(agendamento.Telefone) ||
               string.IsNullOrWhiteSpace(agendamento.TipoVeiculo) ||
               string.IsNullOrWhiteSpace(agendamento.ModeloVeiculo) ||
               string.IsNullOrWhiteSpace(agendamento.PlacaVeiculo) ||
               string.IsNullOrWhiteSpace(agendamento.TipoServico) ||
               string.IsNullOrWhiteSpace(agendamento.Funcionario))
            {
                await DisplayAlert("Erro", "Todos os campos devem ser preenchidos!", "OK");
                return;
            }

            try
            {
                // Salvando no banco de dados
                await _servicoConexao.Salvar(agendamento);
                await DisplayAlert("Sucesso", "Lavagem registrada com sucesso!", "OK");

                // Atualiza lista
                CarregaLista();
                LimparCampos();

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao salvar agendamento: {ex.Message}", "OK");
            }
        }

        private void LimparCampos()
        {
            txtProprietario.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            cbxVeiculo.SelectedItem = null;
            txtModelo.Text = string.Empty;
            txtPlaca.Text = string.Empty;
            cbxServico.SelectedItem = null;
            txtFuncionario.Text = string.Empty;
        }

        private void BTNLimparCampos_Clicked(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void CVLista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (deslizar) return;

            var agendamento = e.CurrentSelection.FirstOrDefault() as Agendamento;
            if (agendamento != null)
            {
                DisplayAlert("Detalhes do Agendamento",
                    $"Proprietário: {agendamento.Proprietario}\n" +
                    $"Telefone: {agendamento.Telefone}\n" +
                    $"Veículo: {agendamento.TipoVeiculo} - {agendamento.ModeloVeiculo} ({agendamento.PlacaVeiculo})\n" +
                    $"Serviço: {agendamento.TipoServico}\n" +
                    $"Funcionário: {agendamento.Funcionario}",
                    "OK");
            }
        }

        private void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
        {
            deslizar = true;
        }

        private void SwipeView_SwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            deslizar = false;
        }

        private async void SwipeItem_Invoked(object sender, EventArgs e)
        {
            var item = ((SwipeItem)sender).BindingContext as Agendamento;

            var resposta = await DisplayAlert("Confirmação", "Deseja realmente apagar o contato ?", "Sim", "Não");

            if (resposta)
            {
                await _servicoConexao.DeletarAgendamento(item);

                await DisplayAlert("Sucesso", "Agendamento Apagado Com Sucesso", "OK");

                AgendamentosEmAndamento.Remove(item);
            }
            else
            {
                await DisplayAlert("Cancelado", "Operação Cancelada", "OK");
            }
        }

        private async Task<string> ExportarTabela()
        {
            try
            {
                // Obtenha todos os agendamentos do banco
                var agendamentos = await _servicoConexao.GetTodosAgendamentos();

                // Defina o caminho do arquivo
                var caminhoArquivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "agendamentos.csv");

                using (var writer = new StreamWriter(caminhoArquivo))
                {
                    // Escreve cabeçalho
                    await writer.WriteLineAsync("Proprietario,Telefone,TipoVeiculo,ModeloVeiculo,PlacaVeiculo,TipoServico,Funcionario,Data");

                    // Escreve os dados
                    foreach (var agendamento in agendamentos)
                    {
                        await writer.WriteLineAsync($"{agendamento.Proprietario},{agendamento.Telefone},{agendamento.TipoVeiculo},{agendamento.ModeloVeiculo},{agendamento.PlacaVeiculo},{agendamento.TipoServico},{agendamento.Funcionario},{agendamento.Data}");
                    }
                }

                return caminhoArquivo;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao exportar tabela: {ex.Message}", "OK");
                return null;
            }
        }

        private async void BTNEnviarPorEmail_Clicked(object sender, EventArgs e)
        {
            try
            {

                // Verifica se o campo de email está preenchido
                var emailDestinatario = TxtCampoEmail.Text;
                if (string.IsNullOrWhiteSpace(emailDestinatario))
                {
                    await DisplayAlert("Erro", "Por favor, insira um endereço de email válido.", "OK");
                    return;
                }


                // Exporta a tabela para CSV
                var caminhoArquivo = await ExportarTabela();
                if (string.IsNullOrWhiteSpace(caminhoArquivo))
                    return;

                // Cria um novo email
                var mensagem = new EmailMessage
                {
                    Subject = "Exportação de Agendamentos",
                    Body = "Segue em anexo o arquivo com os agendamentos exportados.",
                    To = new List<string> { emailDestinatario } // Usa o email inserido no campo
                };

                // Anexa o arquivo CSV
                mensagem.Attachments.Add(new EmailAttachment(caminhoArquivo));

                // Envia o email
                await Email.ComposeAsync(mensagem);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao enviar email: {ex.Message}", "OK");
            }
        }


    }
}
