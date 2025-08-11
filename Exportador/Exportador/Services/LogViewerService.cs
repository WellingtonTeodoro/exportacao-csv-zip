using CommunityToolkit.Mvvm.ComponentModel;  
using Exportador.Application.Interfaces;      
using Exportador.UI.Interfaces;              
using System.Text;

namespace Exportador.UI.Services
{
    /// <summary>
    /// Serviço que exibe e gerencia a captura de logs em tempo real na interface do usuário.
    /// Implementa <see cref="ILogViewerService"/>, notificações reativas com <see cref="ObservableObject"/>, 
    /// e gerenciamento de recursos com <see cref="IDisposable"/>.
    /// </summary>
    public partial class LogViewerService : ObservableObject, ILogViewerService, IDisposable
    {
        /// <summary>
        /// Serviço de log que dispara eventos contendo mensagens de log.
        /// </summary>
        private readonly ILogService _logService;

        /// <summary>
        /// Acumulador interno usado para compor o conteúdo completo do log atual.
        /// </summary>
        private readonly StringBuilder _logBuilder = new StringBuilder();

        /// <summary>
        /// Indica se a captura de logs está atualmente ativa.
        /// </summary>
        private bool _isCapturingLogs = false;

        /// <summary>
        /// Indica se a instância já foi descartada via Dispose/>.
        /// </summary>
        private bool _isDisposed = false;

        /// <summary>
        /// Conteúdo atual do log exibido na interface do usuário.
        /// Atualizado em tempo real durante a captura.
        /// </summary>
        [ObservableProperty]
        private string _currentLog = string.Empty;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="LogViewerService"/>.
        /// </summary>
        /// <param name="logService">Instância do serviço de log usado como fonte de mensagens.</param>
        /// <exception cref="ArgumentNullException">Lançado se <paramref name="logService"/> for nulo.</exception>
        public LogViewerService(ILogService logService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _logService.LogReceived += OnLogReceived;
        }

        /// <summary>
        /// Manipula eventos de log recebidos do <see cref="ILogService"/>.
        /// Se a captura estiver ativa, adiciona a mensagem ao log acumulado.
        /// </summary>
        /// <param name="message">Mensagem de log recebida.</param>
        private void OnLogReceived(string message)
        {
            if (_isDisposed) return;

            if (_isCapturingLogs)
            {
                if (_logBuilder.Length > 0)
                    _logBuilder.AppendLine();

                _logBuilder.Append(message);
                CurrentLog = _logBuilder.ToString(); 
            }
        }

        /// <summary>
        /// Inicia a captura de mensagens de log.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Se o serviço já tiver sido descartado.</exception>
        public void StartCapturing()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(LogViewerService), "Não é possível iniciar a captura de logs em um serviço descartado.");

            _isCapturingLogs = true; 
        }

        /// <summary>
        /// Interrompe a captura de mensagens de log.
        /// </summary>
        public void StopCapturing()
        {
            if (_isDisposed)
                return;

            _isCapturingLogs = false;
        }

        /// <summary>
        /// Limpa todo o conteúdo atual do log.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Se o serviço já tiver sido descartado.</exception>
        public void ClearLog()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(LogViewerService), "Não é possível limpar o logon em um serviço descartado.");

            _logBuilder.Clear();
            CurrentLog = string.Empty;
        }

        /// <summary>
        /// Libera os recursos usados por esta instância de forma explícita.
        /// Remove a inscrição no evento de log e marca o objeto como descartado.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  
        }

        /// <summary>
        /// Libera recursos gerenciados e não gerenciados.
        /// </summary>
        /// <param name="disposing">
        /// True se chamado manualmente via Dispose/>;
        /// False se chamado pelo finalizador (caso implementado).
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            { 
                if (_logService != null)
                {
                    _logService.LogReceived -= OnLogReceived;
                }
            } 

            _isDisposed = true;
        } 
    }
}
