using CSharpFunctionalExtensions;
using Exportador.Application.Interfaces;

namespace Exportador.Infrastructure.Repositories;

/// <summary>
/// Uma implementação em memória do repositório de mapeamento de exportação. 
/// </summary>
public class InMemoryExportMappingRepository : IExportMappingRepository
{ 
    private readonly Dictionary<string, (string RealTableName, string SelectColumns)> _tableMapping = new()
    { 
        { "Clientes", ("dbo.ClientesExportacao", "codigo, razao_social, nome_fantasia, tipo, cpf, cnpj, tipo_contribuinte, inscricao_estadual, inscricao_municipal, situacao, regime_tributario, data_nascimento, numero_pis_inss, sexo, observacao, data_cadastro, cliente, fornecedor, vendedor, entregador, temp_data_ultima_alteracao, email, email_principal, tipo_endereco, endereco, numero, complemento, bairro, uf_cidade, uid_cidade, cep, referencia, tipo_telefone, telefone, nome_contato, telefone_principal") },
        { "Produtos", ("dbo.ProdutosExportacao", "codigo, descricao, codigo_barras, referencia, ativo, uid_grupo, uid_marca, uid_unidade_medida, preco_custo, margem_lucro, preco_venda, id_ncm, cest, estoque, codigo_anp, percentual_glp_petroleo, percentual_gas_nacional, percentual_gas_importado, valor_partida, uid_unidade_tributavel, quantidade_unidade_tributavel, data_ultima_atualizacao, codigo_balanca, departamento") },
        { "ContasReceber", ("dbo.ContasReceberExportacao", "numero_parcela, total_parcela, uid_participante, data_emissao, data_vencimento_original, data_vencimento, valor_crediario, valor_juro, valor_desconto, valor_pagar, valor_pago, valor_saldo, uid_usuario_inclusao, data_hora_inclusao, uid_usuario_alteracao, data_hora_alteracao, data_hora_cancelamento, uid_usuario_cancelamento, data_quitacao, codigo, observacao_crediario") },
        { "ContasPagar", ("dbo.ContasPagarExportacao", "numero_parcela, total_parcela, uid_fornecedor, data_emissao, data_vencimento_original, data_vencimento, valor_crediario, valor_juro, valor_desconto, valor_pagar, valor_pago, valor_saldo, uid_usuario_inclusao, data_hora_inclusao, uid_usuario_alteracao, data_hora_alteracao, data_hora_cancelamento, uid_usuario_cancelamento, data_quitacao, codigo, observacao_contas_pagar") },
        { "NFe", ("dbo.NFeExportacao", "cnpj_emitente, numero_documento,codigo_numerico, chave, protocolo, recebido_em, xml_autorizado") },
        { "NFCe", ("dbo.NFCeExportacao", "serie, numero_documento, codigo_numerico, chave, tag_id, protocolo, recebido_em, xml_autorizado") },
        { "Notas", ("dbo.NotasExportacao", "cnpj_fornecedor, uid_fornecedor, numero_documento, serie, chave, xml_final") }
    };

    /// <inheritdoc/>
    public Maybe<(string RealTableName, string SelectColumns)> GetMapping(string friendlyName)
    {
        return _tableMapping.TryGetValue(friendlyName, out var mapping) ? Maybe.From(mapping) : Maybe.None;
    }
}