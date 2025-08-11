using Exportador.Application.Interfaces;
using Exportador.Application.Models;

namespace Exportador.Infrastructure.Configuration;

/// <summary>
/// Fornece as definições SQL para as views de banco de dados necessárias
/// especificamente para o SQL Server.
/// Esta classe centraliza as strings SQL de criação de views, separando-as
/// da lógica de aplicação principal.
/// </summary>
public class SqlServerViewDefinitions : IViewDefinitionProvider
{
    /// <summary>
    /// Obtém uma coleção somente leitura de todas as views obrigatórias,
    /// incluindo seus nomes e suas respectivas definições SQL.
    /// </summary>
    /// <remarks>
    /// Este é o local para adicionar, remover ou modificar as definições SQL das views.
    /// As views listadas aqui serão criadas/alteradas no banco de dados de destino.
    /// </remarks> 
    public IReadOnlyList<DatabaseViewDefinition> GetRequiredViews()
    {
        return new List<DatabaseViewDefinition>
        {
            new DatabaseViewDefinition("dbo.ClientesExportacao", "SELECT [codigo] ,[razao_social] ,[nome_fantasia] ,[tipo] ,[cpf] ,[cnpj] ,[tipo_contribuinte] ,[inscricao_estadual] ,[inscricao_municipal] ,[situacao] ,[regime_tributario] ,[data_nascimento] ,[numero_pis_inss] ,[sexo] ,[observacao] ,[data_cadastro] ,[cliente] ,[fornecedor] ,[vendedor] ,[entregador] ,[temp_data_ultima_alteracao] ,[email] ,[email_principal] ,[tipo_endereco] ,[endereco] ,[numero] ,[complemento] ,[bairro] ,[uf_cidade] ,[uid_cidade] ,[cep] ,[referencia] ,[tipo_telefone] ,[telefone] ,[nome_contato] ,[telefone_principal] FROM [dbo].[Clientes]"),
            new DatabaseViewDefinition("dbo.ProdutosExportacao", "SELECT [codigo] ,[descricao] ,[codigo_barras] ,[referencia] ,[ativo] ,[uid_grupo] ,[uid_marca] ,[uid_unidade_medida] ,[preco_custo] ,[margem_lucro] ,[preco_venda] ,[id_ncm] ,[cest] ,[estoque] ,[codigo_anp] ,[percentual_glp_petroleo] ,[percentual_gas_nacional] ,[percentual_gas_importado] ,[valor_partida] ,[uid_unidade_tributavel] ,[quantidade_unidade_tributavel] ,[data_ultima_atualizacao] ,[codigo_balanca] ,[departamento] FROM [dbo].[Produtos] "),
            new DatabaseViewDefinition("dbo.ContasReceberExportacao", "SELECT [numero_parcela] ,[total_parcela] ,[uid_participante] ,[data_emissao] ,[data_vencimento_original] ,[data_vencimento] ,[valor_crediario] ,[valor_juro] ,[valor_desconto] ,[valor_pagar] ,[valor_pago] ,[valor_saldo] ,[uid_usuario_inclusao] ,[data_hora_inclusao] ,[uid_usuario_alteracao] ,[data_hora_alteracao] ,[data_hora_cancelamento] ,[uid_usuario_cancelamento] ,[data_quitacao] ,[codigo] ,[observacao_crediario] FROM [dbo].[ContasReceber] "),
            new DatabaseViewDefinition("dbo.ContasPagarExportacao", "SELECT [numero_parcela] ,[total_parcela] ,[uid_fornecedor] ,[data_emissao] ,[data_vencimento_original] ,[data_vencimento] ,[valor_crediario] ,[valor_juro] ,[valor_desconto] ,[valor_pagar] ,[valor_pago] ,[valor_saldo] ,[uid_usuario_inclusao] ,[data_hora_inclusao] ,[uid_usuario_alteracao] ,[data_hora_alteracao] ,[data_hora_cancelamento] ,[uid_usuario_cancelamento] ,[data_quitacao] ,[codigo] ,[observacao_contas_pagar] FROM [dbo].[ContasPagar]"),
            new DatabaseViewDefinition("dbo.NFeExportacao", "SELECT [cnpj_emitente] ,[numero_documento] ,[codigo_numerico] ,[chave] ,[protocolo] ,[recebido_em] ,[xml_autorizado] FROM [dbo].[NFe]"),
            new DatabaseViewDefinition("dbo.NFCeExportacao", "SELECT [serie] ,[numero_documento] ,[codigo_numerico] ,[chave] ,[tag_id] ,[protocolo] ,[recebido_em] ,[xml_autorizado] FROM [dbo].[NFCe]"),
            new DatabaseViewDefinition("dbo.NotasExportacao", "SELECT [cnpj_fornecedor] ,[uid_fornecedor] ,[numero_documento] ,[serie] ,[chave] ,[xml_final] FROM [dbo].[Notas] ")
        };
    }
}
