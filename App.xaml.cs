using LavaRapidoMobile.Classes;

namespace LavaRapidoMobile
{
    public partial class App : Application
    {
        public static Servico Conexao { get; private set; }

        public App()
        {
            Conexao = new Servico();

            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}
