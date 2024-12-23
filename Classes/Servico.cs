using SQLite;

namespace LavaRapidoMobile.Classes
{
    public class Servico
    {
        private readonly SQLiteAsyncConnection _conexao;

        public Servico()
        {
            //caminho do banco de dados
            var localBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "agendamento.db3");

            //inicia a conexão do SQLite
            _conexao = new SQLiteAsyncConnection(localBD);

            //cria a tabela caso não existe
            _conexao.CreateTableAsync<Agendamento>().Wait();

        }

        public Task<int> Salvar(Agendamento agendamento)
        {
            if(agendamento == null)
            {
                throw new ArgumentNullException(nameof(agendamento), "Agendamento não pode ser nulo.");
            }
            return _conexao.InsertAsync(agendamento);
        }

        public Task<List<Agendamento>> GetTodosAgendamentos()
        {
            return _conexao.Table<Agendamento>().ToListAsync();
        }

        public async Task<bool> DeletarAgendamento(Agendamento agendamento)
        {
            try
            {
                await _conexao.DeleteAsync(agendamento);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
