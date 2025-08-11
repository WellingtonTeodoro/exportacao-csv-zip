INSERT INTO dbo.Clientes (
    codigo, razao_social, nome_fantasia, tipo, cpf, cnpj, tipo_contribuinte,
    inscricao_estadual, inscricao_municipal, situacao, regime_tributario, data_nascimento,
    numero_pis_inss, sexo, observacao, data_cadastro, cliente, fornecedor,
    vendedor, entregador, temp_data_ultima_alteracao, email, email_principal, tipo_endereco,
    endereco, numero, complemento, bairro, uf_cidade, uid_cidade, cep,
    referencia, tipo_telefone, telefone, nome_contato, telefone_principal
) VALUES
(1, 'ACME Indústrias Ltda', 'Acme', 'J', '', '12345678000101', '1', '123456789', '987654321', 1, '3', CONVERT(DATETIME, '1980-01-01', 120), '', 'M', 'Cliente VIP', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'contato@acme.com', 1, '1', 'Rua das Flores', '100', 'Apto 101', 'Centro', 'SP', '550e8400-e29b-41d4-a716-446655440000', '01001000', '', 1, '11999999999', 'João Silva', 1),
(2, 'Beta Comércio de Produtos', 'Beta Produtos', 'J', '', '22345678000102', '2', '223456789', '887654321', 1, '3', CONVERT(DATETIME, '1975-05-05', 120), '', 'F', 'Cliente novo', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'contato@beta.com', 1, '1', 'Av. Brasil', '200', '', 'Vila Nova', 'RJ', '660e8400-e29b-41d4-a716-446655440001', '20020000', '', 1, '21988888888', 'Maria Souza', 1),
(3, 'Carlos Ferreira', 'Carlos', 'F', '12345678900', '', '1', '', '', 1, '3', CONVERT(DATETIME, '1990-07-12', 120), '12345678901', 'M', '', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'carlos@gmail.com', 1, '1', 'Rua do Sol', '300', 'Casa', 'Jardim', 'MG', '750e8400-e29b-41d4-a716-446655440002', '30140071', '', 1, '31977777777', 'Carlos Filho', 1),
(4, 'Delta Serviços Ltda', 'Delta Serviços', 'J', '', '32345678000103', '1', '323456789', '787654321', 1, '3', CONVERT(DATETIME, '1985-11-23', 120), '', 'F', 'Cliente antigo', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'contato@delta.com', 1, '1', 'Rua das Palmeiras', '400', 'Sala 2', 'Bela Vista', 'RS', '850e8400-e29b-41d4-a716-446655440003', '90020000', '', 1, '5133333333', 'Ana Paula', 1),
(5, 'Eduardo Lima', 'Eduardo', 'F', '22345678900', '', '2', '', '', 1, '3', CONVERT(DATETIME, '1992-03-30', 120), '22345678902', 'M', '', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'eduardo@gmail.com', 1, '1', 'Av. Ipiranga', '500', '', 'Centro', 'PR', '950e8400-e29b-41d4-a716-446655440004', '80010000', '', 1, '41922222222', 'Eduardo Filho', 1),
(6, 'Fênix Importações', 'Fênix', 'J', '', '42345678000104', '1', '423456789', '687654321', 1, '3', CONVERT(DATETIME, '1979-08-15', 120), '', 'M', 'Cliente especial', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'contato@fenix.com', 1, '1', 'Rua Afonso Pena', '600', '', 'Centro', 'BA', '720e8400-e29b-41d4-a716-446655440005', '40020000', '', 1, '7195555555', 'Lucas Santos', 1),
(7, 'Gustavo Pereira', 'Gustavo', 'F', '32345678900', '', '2', '', '', 1, '3', CONVERT(DATETIME, '1988-12-22', 120), '32345678903', 'M', '', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'gustavo@gmail.com', 1, '1', 'Rua XV de Novembro', '700', '', 'Centro', 'SC', '770e8400-e29b-41d4-a716-446655440006', '88010000', '', 1, '4799999999', 'Gustavo Filho', 1),
(8, 'Hitech Solutions', 'Hitech', 'J', '', '52345678000105', '1', '523456789', '587654321', 1, '3', CONVERT(DATETIME, '1982-09-05', 120), '', 'F', '', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'contato@hitech.com', 1, '1', 'Av. Paulista', '800', 'Conj 12', 'Bela Vista', 'SP', '660e8400-e29b-41d4-a716-446655440007', '01311000', '', 1, '1133333333', 'Fernanda Alves', 1),
(9, 'Isabela Gomes', 'Isabela', 'F', '42345678900', '', '2', '', '', 1, '3', CONVERT(DATETIME, '1995-06-17', 120), '42345678904', 'F', '', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'isabela@gmail.com', 1, '1', 'Rua dos Andradas', '900', '', 'Centro', 'RS', '850e8400-e29b-41d4-a716-446655440008', '90020001', '', 1, '5132222222', 'Isabela Filho', 1),
(10, 'Júlio César LTDA', 'Júlio César', 'J', '', '62345678000106', '1', '623456789', '487654321', 1, '3', CONVERT(DATETIME, '1970-10-10', 120), '', 'M', 'Cliente importante', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'contato@julio.com', 1, '1', 'Rua Sete de Setembro', '1000', 'Sala 10', 'Centro', 'PE', '910e8400-e29b-41d4-a716-446655440009', '50010000', '', 1, '8133333333', 'Júlio Filho', 1),
(11, 'Karen Alves', 'Karen', 'F', '52345678900', '', '2', '', '', 1, '3', CONVERT(DATETIME, '1991-02-28', 120), '52345678905', 'F', '', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'karen@gmail.com', 1, '1', 'Rua Marechal Deodoro', '1100', '', 'Centro', 'CE', '730e8400-e29b-41d4-a716-446655440010', '60020000', '', 1, '8533333333', 'Karen Filho', 1),
(12, 'Lima & Cia', 'Lima', 'J', '', '72345678000107', '1', '723456789', '387654321', 1, '3', CONVERT(DATETIME, '1987-07-07', 120), '', 'M', '', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'contato@lima.com', 1, '1', 'Av. Sete de Setembro', '1200', '', 'Centro', 'MG', '950e8400-e29b-41d4-a716-446655440011', '30120000', '', 1, '3194444444', 'Paulo Lima', 1),
(13, 'Márcia Rodrigues', 'Márcia', 'F', '62345678900', '', '2', '', '', 1, '3', CONVERT(DATETIME, '1984-11-11', 120), '62345678906', 'F', '', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'marcia@gmail.com', 1, '1', 'Rua Barão do Rio Branco', '1300', '', 'Centro', 'RJ', '670e8400-e29b-41d4-a716-446655440012', '20040000', '', 1, '2194444444', 'Márcia Filho', 1),
(14, 'NorteTech Ltda', 'NorteTech', 'J', '', '82345678000108', '1', '823456789', '287654321', 1, '3', CONVERT(DATETIME, '1981-03-03', 120), '', 'M', '', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'contato@nortetech.com', 1, '1', 'Av. Rio Branco', '1400', 'Sala 3', 'Centro', 'PA', '770e8400-e29b-41d4-a716-446655440013', '66010000', '', 1, '9195555555', 'Carlos Norte', 1),
(15, 'Oliveira & Filhos', 'Oliveira', 'J', '', '92345678000109', '2', '923456789', '187654321', 1, '3', CONVERT(DATETIME, '1978-12-12', 120), '', 'F', 'Cliente recorrente', CONVERT(DATETIME, '2025-01-15', 120), 1, 0, 0, 0, '', 'contato@oliveira.com', 1, '1', 'Rua da Paz', '1500', '', 'Centro', 'PR', '660e8400-e29b-41d4-a716-446655440014', '80020000', '', 1, '4195555555', 'Fernanda Oliveira', 1);

INSERT INTO dbo.Produtos (
    codigo, descricao, codigo_barras, referencia, ativo, uid_grupo, uid_marca, uid_unidade_medida,
    preco_custo, margem_lucro, preco_venda, id_ncm, cest, estoque, codigo_anp, percentual_glp_petroleo,
    percentual_gas_nacional, percentual_gas_importado, valor_partida, uid_unidade_tributavel,
    quantidade_unidade_tributavel, data_ultima_atualizacao, codigo_balanca, departamento
) VALUES
(1, 'Produto A', '7891234560012', 'REF001', 1, '550e8400-e29b-41d4-a716-446655440000', '111e8400-e29b-41d4-a716-446655440001', '222e8400-e29b-41d4-a716-446655440002', 10.0, 20.0, 12.0, '1000', '010203', 100, 'ANP001', 5.0, 3.0, 2.0, 50.0, '333e8400-e29b-41d4-a716-446655440003', 1, CONVERT(DATETIME, '2025-01-15', 120), 'CB001', 1),
(2, 'Produto B', '7891234560013', 'REF002', 1, '550e8400-e29b-41d4-a716-446655440010', '111e8400-e29b-41d4-a716-446655440011', '222e8400-e29b-41d4-a716-446655440012', 20.0, 15.0, 22.0, '1001', '010204', 50, 'ANP002', 4.0, 2.0, 1.0, 30.0, '333e8400-e29b-41d4-a716-446655440013', 2, CONVERT(DATETIME, '2025-01-15', 120), 'CB002', 1),
(3, 'Produto C', '7891234560014', 'REF003', 1, '550e8400-e29b-41d4-a716-446655440020', '111e8400-e29b-41d4-a716-446655440021', '222e8400-e29b-41d4-a716-446655440022', 15.0, 18.0, 17.5, '1002', '010205', 75, 'ANP003', 6.0, 4.0, 3.0, 40.0, '333e8400-e29b-41d4-a716-446655440023', 3, CONVERT(DATETIME, '2025-01-15', 120), 'CB003', 1),
(4, 'Produto D', '7891234560015', 'REF004', 1, '550e8400-e29b-41d4-a716-446655440030', '111e8400-e29b-41d4-a716-446655440031', '222e8400-e29b-41d4-a716-446655440032', 12.0, 25.0, 15.0, '1003', '010206', 60, 'ANP004', 7.0, 3.5, 2.5, 55.0, '333e8400-e29b-41d4-a716-446655440033', 4, CONVERT(DATETIME, '2025-01-15', 120), 'CB004', 1),
(5, 'Produto E', '7891234560016', 'REF005', 1, '550e8400-e29b-41d4-a716-446655440040', '111e8400-e29b-41d4-a716-446655440041', '222e8400-e29b-41d4-a716-446655440042', 8.0, 30.0, 10.0, '1004', '010207', 90, 'ANP005', 5.5, 2.5, 1.5, 60.0, '333e8400-e29b-41d4-a716-446655440043', 5, CONVERT(DATETIME, '2025-01-15', 120), 'CB005', 1),
(6, 'Produto F', '7891234560017', 'REF006', 1, '550e8400-e29b-41d4-a716-446655440050', '111e8400-e29b-41d4-a716-446655440051', '222e8400-e29b-41d4-a716-446655440052', 14.0, 10.0, 15.0, '1005', '010208', 40, 'ANP006', 3.0, 1.5, 0.5, 25.0, '333e8400-e29b-41d4-a716-446655440053', 6, CONVERT(DATETIME, '2025-01-15', 120), 'CB006', 1),
(7, 'Produto G', '7891234560018', 'REF007', 1, '550e8400-e29b-41d4-a716-446655440060', '111e8400-e29b-41d4-a716-446655440061', '222e8400-e29b-41d4-a716-446655440062', 11.0, 22.0, 13.5, '1006', '010209', 30, 'ANP007', 4.0, 2.0, 1.0, 35.0, '333e8400-e29b-41d4-a716-446655440063', 7, CONVERT(DATETIME, '2025-01-15', 120), 'CB007', 1),
(8, 'Produto H', '7891234560019', 'REF008', 1, '550e8400-e29b-41d4-a716-446655440070', '111e8400-e29b-41d4-a716-446655440071', '222e8400-e29b-41d4-a716-446655440072', 9.0, 28.0, 11.5, '1007', '010210', 80, 'ANP008', 6.0, 3.0, 2.0, 45.0, '333e8400-e29b-41d4-a716-446655440073', 8, CONVERT(DATETIME, '2025-01-15', 120), 'CB008', 1),
(9, 'Produto I', '7891234560020', 'REF009', 1, '550e8400-e29b-41d4-a716-446655440080', '111e8400-e29b-41d4-a716-446655440081', '222e8400-e29b-41d4-a716-446655440082', 16.0, 12.0, 18.0, '1008', '010211', 20, 'ANP009', 7.0, 4.0, 3.0, 55.0, '333e8400-e29b-41d4-a716-446655440083', 9, CONVERT(DATETIME, '2025-01-15', 120), 'CB009', 1),
(10, 'Produto J', '7891234560021', 'REF010', 1, '550e8400-e29b-41d4-a716-446655440090', '111e8400-e29b-41d4-a716-446655440091', '222e8400-e29b-41d4-a716-446655440092', 13.0, 20.0, 15.5, '1009', '010212', 50, 'ANP010', 5.0, 2.5, 1.5, 40.0, '333e8400-e29b-41d4-a716-446655440093', 10, CONVERT(DATETIME, '2025-01-15', 120), 'CB010', 1),
(11, 'Produto K', '7891234560022', 'REF011', 1, '550e8400-e29b-41d4-a716-446655440100', '111e8400-e29b-41d4-a716-446655440101', '222e8400-e29b-41d4-a716-446655440102', 14.5, 19.0, 16.5, '1010', '010213', 70, 'ANP011', 6.0, 3.5, 2.5, 50.0, '333e8400-e29b-41d4-a716-446655440103', 11, CONVERT(DATETIME, '2025-01-15', 120), 'CB011', 1),
(12, 'Produto L', '7891234560023', 'REF012', 1, '550e8400-e29b-41d4-a716-446655440110', '111e8400-e29b-41d4-a716-446655440111', '222e8400-e29b-41d4-a716-446655440112', 10.5, 24.0, 13.0, '1011', '010214', 65, 'ANP012', 5.5, 2.5, 1.5, 48.0, '333e8400-e29b-41d4-a716-446655440113', 12, CONVERT(DATETIME, '2025-01-15', 120), 'CB012', 1),
(13, 'Produto M', '7891234560024', 'REF013', 1, '550e8400-e29b-41d4-a716-446655440120', '111e8400-e29b-41d4-a716-446655440121', '222e8400-e29b-41d4-a716-446655440122', 12.5, 16.0, 14.5, '1012', '010215', 55, 'ANP013', 4.5, 2.0, 1.0, 43.0, '333e8400-e29b-41d4-a716-446655440123', 13, CONVERT(DATETIME, '2025-01-15', 120), 'CB013', 1),
(14, 'Produto N', '7891234560025', 'REF014', 1, '550e8400-e29b-41d4-a716-446655440130', '111e8400-e29b-41d4-a716-446655440131', '222e8400-e29b-41d4-a716-446655440132', 11.0, 21.0, 13.5, '1013', '010216', 75, 'ANP014', 6.0, 3.0, 2.0, 46.0, '333e8400-e29b-41d4-a716-446655440133', 14, CONVERT(DATETIME, '2025-01-15', 120), 'CB014', 1);


INSERT INTO dbo.ContasReceber (
    uid_crediario_proprio, uid_empresa, uid_integracao, uid_venda, numero_parcela, total_parcela,
    uid_participante, data_emissao, data_vencimento_original, data_vencimento, valor_crediario,
    valor_juro, valor_desconto, valor_pagar, valor_pago, valor_saldo, uid_usuario_inclusao,
    data_hora_inclusao, uid_usuario_alteracao, data_hora_alteracao, data_hora_cancelamento,
    uid_usuario_cancelamento, data_quitacao, codigo, observacao_crediario
) VALUES
('uidcp001', 'uidemp001', 'uidint001', 'uidv001', '1', '3', 'uidpart001', CONVERT(DATETIME, '2025-01-01', 120), CONVERT(DATETIME, '2025-01-15', 120), CONVERT(DATETIME, '2025-01-15', 120), 1000.00, 10.00, 5.00, 1005.00, 1000.00, 5.00, 'uiduser001', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-01-15', 120), 1, ''),
('uidcp002', 'uidemp002', 'uidint002', 'uidv002', '1', '2', 'uidpart002', CONVERT(DATETIME, '2025-02-01', 120), CONVERT(DATETIME, '2025-02-15', 120), CONVERT(DATETIME, '2025-02-15', 120), 1500.00, 15.00, 10.00, 1505.00, 1500.00, 5.00, 'uiduser002', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-02-15', 120), 2, ''),
('uidcp003', 'uidemp003', 'uidint003', 'uidv003', '2', '4', 'uidpart003', CONVERT(DATETIME, '2025-03-01', 120), CONVERT(DATETIME, '2025-03-15', 120), CONVERT(DATETIME, '2025-03-15', 120), 2000.00, 20.00, 15.00, 2005.00, 2000.00, 5.00, 'uiduser003', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-03-15', 120), 3, ''),
('uidcp004', 'uidemp004', 'uidint004', 'uidv004', '1', '1', 'uidpart004', CONVERT(DATETIME, '2025-04-01', 120), CONVERT(DATETIME, '2025-04-15', 120), CONVERT(DATETIME, '2025-04-15', 120), 2500.00, 25.00, 20.00, 2505.00, 2500.00, 5.00, 'uiduser004', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-04-15', 120), 4, ''),
('uidcp005', 'uidemp005', 'uidint005', 'uidv005', '3', '3', 'uidpart005', CONVERT(DATETIME, '2025-05-01', 120), CONVERT(DATETIME, '2025-05-15', 120), CONVERT(DATETIME, '2025-05-15', 120), 3000.00, 30.00, 25.00, 3005.00, 3000.00, 5.00, 'uiduser005', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-05-15', 120), 5, ''),
('uidcp006', 'uidemp006', 'uidint006', 'uidv006', '1', '5', 'uidpart006', CONVERT(DATETIME, '2025-06-01', 120), CONVERT(DATETIME, '2025-06-15', 120), CONVERT(DATETIME, '2025-06-15', 120), 3500.00, 35.00, 30.00, 3505.00, 3500.00, 5.00, 'uiduser006', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-06-15', 120), 6, ''),
('uidcp007', 'uidemp007', 'uidint007', 'uidv007', '2', '3', 'uidpart007', CONVERT(DATETIME, '2025-07-01', 120), CONVERT(DATETIME, '2025-07-15', 120), CONVERT(DATETIME, '2025-07-15', 120), 4000.00, 40.00, 35.00, 4005.00, 4000.00, 5.00, 'uiduser007', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-07-15', 120), 7, ''),
('uidcp008', 'uidemp008', 'uidint008', 'uidv008', '1', '2', 'uidpart008', CONVERT(DATETIME, '2025-08-01', 120), CONVERT(DATETIME, '2025-08-15', 120), CONVERT(DATETIME, '2025-08-15', 120), 4500.00, 45.00, 40.00, 4505.00, 4500.00, 5.00, 'uiduser008', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-08-15', 120), 8, ''),
('uidcp009', 'uidemp009', 'uidint009', 'uidv009', '3', '1', 'uidpart009', CONVERT(DATETIME, '2025-09-01', 120), CONVERT(DATETIME, '2025-09-15', 120), CONVERT(DATETIME, '2025-09-15', 120), 5000.00, 50.00, 45.00, 5005.00, 5000.00, 5.00, 'uiduser009', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-09-15', 120), 9, ''),
('uidcp010', 'uidemp010', 'uidint010', 'uidv010', '2', '4', 'uidpart010', CONVERT(DATETIME, '2025-10-01', 120), CONVERT(DATETIME, '2025-10-15', 120), CONVERT(DATETIME, '2025-10-15', 120), 5500.00, 55.00, 50.00, 5505.00, 5500.00, 5.00, 'uiduser010', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-10-15', 120), 10, ''),
('uidcp011', 'uidemp011', 'uidint011', 'uidv011', '1', '3', 'uidpart011', CONVERT(DATETIME, '2025-11-01', 120), CONVERT(DATETIME, '2025-11-15', 120), CONVERT(DATETIME, '2025-11-15', 120), 6000.00, 60.00, 55.00, 6005.00, 6000.00, 5.00, 'uiduser011', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-11-15', 120), 11, ''),
('uidcp012', 'uidemp012', 'uidint012', 'uidv012', '3', '2', 'uidpart012', CONVERT(DATETIME, '2025-12-01', 120), CONVERT(DATETIME, '2025-12-15', 120), CONVERT(DATETIME, '2025-12-15', 120), 6500.00, 65.00, 60.00, 6505.00, 6500.00, 5.00, 'uiduser012', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-12-15', 120), 12, ''),
('uidcp013', 'uidemp013', 'uidint013', 'uidv013', '2', '3', 'uidpart013', CONVERT(DATETIME, '2026-01-01', 120), CONVERT(DATETIME, '2026-01-15', 120), CONVERT(DATETIME, '2026-01-15', 120), 7000.00, 70.00, 65.00, 7005.00, 7000.00, 5.00, 'uiduser013', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2026-01-15', 120), 13, '');


INSERT INTO dbo.ContasPagar (
    uid_contas_pagar, uid_empresa, uid_integracao, uid_compra, numero_parcela, total_parcela,
    uid_fornecedor, data_emissao, data_vencimento_original, data_vencimento, valor_crediario,
    valor_juro, valor_desconto, valor_pagar, valor_pago, valor_saldo, uid_usuario_inclusao,
    data_hora_inclusao, uid_usuario_alteracao, data_hora_alteracao, data_hora_cancelamento,
    uid_usuario_cancelamento, data_quitacao, codigo, observacao_contas_pagar
) VALUES
('uidcpg001', 'uidemp001', 'uidint001', 'uidc001', '1', '3', 'uidfor001', CONVERT(DATETIME, '2025-01-01', 120), CONVERT(DATETIME, '2025-01-15', 120), CONVERT(DATETIME, '2025-01-15', 120), 800.00, 8.00, 4.00, 804.00, 800.00, 4.00, 'uiduser001', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-01-15', 120), 1, ''),
('uidcpg002', 'uidemp002', 'uidint002', 'uidc002', '2', '2', 'uidfor002', CONVERT(DATETIME, '2025-02-01', 120), CONVERT(DATETIME, '2025-02-15', 120), CONVERT(DATETIME, '2025-02-15', 120), 1000.00, 10.00, 5.00, 1005.00, 1000.00, 5.00, 'uiduser002', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-02-15', 120), 2, ''),
('uidcpg003', 'uidemp003', 'uidint003', 'uidc003', '1', '4', 'uidfor003', CONVERT(DATETIME, '2025-03-01', 120), CONVERT(DATETIME, '2025-03-15', 120), CONVERT(DATETIME, '2025-03-15', 120), 1200.00, 12.00, 6.00, 1206.00, 1200.00, 6.00, 'uiduser003', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-03-15', 120), 3, ''),
('uidcpg004', 'uidemp004', 'uidint004', 'uidc004', '3', '1', 'uidfor004', CONVERT(DATETIME, '2025-04-01', 120), CONVERT(DATETIME, '2025-04-15', 120), CONVERT(DATETIME, '2025-04-15', 120), 900.00, 9.00, 4.50, 904.50, 900.00, 4.50, 'uiduser004', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-04-15', 120), 4, ''),
('uidcpg005', 'uidemp005', 'uidint005', 'uidc005', '1', '5', 'uidfor005', CONVERT(DATETIME, '2025-05-01', 120), CONVERT(DATETIME, '2025-05-15', 120), CONVERT(DATETIME, '2025-05-15', 120), 1300.00, 13.00, 6.50, 1306.50, 1300.00, 6.50, 'uiduser005', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-05-15', 120), 5, ''),
('uidcpg006', 'uidemp006', 'uidint006', 'uidc006', '2', '3', 'uidfor006', CONVERT(DATETIME, '2025-06-01', 120), CONVERT(DATETIME, '2025-06-15', 120), CONVERT(DATETIME, '2025-06-15', 120), 1400.00, 14.00, 7.00, 1407.00, 1400.00, 7.00, 'uiduser006', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-06-15', 120), 6, ''),
('uidcpg007', 'uidemp007', 'uidint007', 'uidc007', '1', '2', 'uidfor007', CONVERT(DATETIME, '2025-07-01', 120), CONVERT(DATETIME, '2025-07-15', 120), CONVERT(DATETIME, '2025-07-15', 120), 1100.00, 11.00, 5.50, 1105.50, 1100.00, 5.50, 'uiduser007', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-07-15', 120), 7, ''),
('uidcpg008', 'uidemp008', 'uidint008', 'uidc008', '3', '1', 'uidfor008', CONVERT(DATETIME, '2025-08-01', 120), CONVERT(DATETIME, '2025-08-15', 120), CONVERT(DATETIME, '2025-08-15', 120), 1600.00, 16.00, 8.00, 1608.00, 1600.00, 8.00, 'uiduser008', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-08-15', 120), 8, ''),
('uidcpg009', 'uidemp009', 'uidint009', 'uidc009', '1', '4', 'uidfor009', CONVERT(DATETIME, '2025-09-01', 120), CONVERT(DATETIME, '2025-09-15', 120), CONVERT(DATETIME, '2025-09-15', 120), 1700.00, 17.00, 8.50, 1708.50, 1700.00, 8.50, 'uiduser009', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-09-15', 120), 9, ''),
('uidcpg010', 'uidemp010', 'uidint010', 'uidc010', '2', '3', 'uidfor010', CONVERT(DATETIME, '2025-10-01', 120), CONVERT(DATETIME, '2025-10-15', 120), CONVERT(DATETIME, '2025-10-15', 120), 1800.00, 18.00, 9.00, 1809.00, 1800.00, 9.00, 'uiduser010', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-10-15', 120), 10, ''),
('uidcpg011', 'uidemp011', 'uidint011', 'uidc011', '1', '2', 'uidfor011', CONVERT(DATETIME, '2025-11-01', 120), CONVERT(DATETIME, '2025-11-15', 120), CONVERT(DATETIME, '2025-11-15', 120), 1900.00, 19.00, 9.50, 1909.50, 1900.00, 9.50, 'uiduser011', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-11-15', 120), 11, ''),
('uidcpg012', 'uidemp012', 'uidint012', 'uidc012', '3', '1', 'uidfor012', CONVERT(DATETIME, '2025-12-01', 120), CONVERT(DATETIME, '2025-12-15', 120), CONVERT(DATETIME, '2025-12-15', 120), 2000.00, 20.00, 10.00, 2010.00, 2000.00, 10.00, 'uiduser012', CONVERT(DATETIME, '2025-01-15', 120), '', NULL, NULL, NULL, CONVERT(DATETIME, '2025-12-15', 120), 12, '');


INSERT INTO dbo.NFe (cnpj_emitente, numero_documento, codigo_numerico, chave, protocolo, recebido_em, xml_autorizado) VALUES
('12345678000199', '1001', 'ABC123', 'CHAVE001', 'PROTO001', CONVERT(DATETIME, '2024-07-01', 120), '<xml>...</xml>'),
('12345678000198', '1002', 'ABC124', 'CHAVE002', 'PROTO002', CONVERT(DATETIME, '2024-07-02', 120), '<xml>...</xml>'),
('12345678000197', '1003', 'ABC125', 'CHAVE003', 'PROTO003', CONVERT(DATETIME, '2024-07-03', 120), '<xml>...</xml>'),
('12345678000196', '1004', 'ABC126', 'CHAVE004', 'PROTO004', CONVERT(DATETIME, '2024-07-04', 120), '<xml>...</xml>'),
('12345678000195', '1005', 'ABC127', 'CHAVE005', 'PROTO005', CONVERT(DATETIME, '2024-07-05', 120), '<xml>...</xml>'),
('12345678000194', '1006', 'ABC128', 'CHAVE006', 'PROTO006', CONVERT(DATETIME, '2024-07-06', 120), '<xml>...</xml>'),
('12345678000193', '1007', 'ABC129', 'CHAVE007', 'PROTO007', CONVERT(DATETIME, '2024-07-07', 120), '<xml>...</xml>'),
('12345678000192', '1008', 'ABC130', 'CHAVE008', 'PROTO008', CONVERT(DATETIME, '2024-07-08', 120), '<xml>...</xml>'),
('12345678000191', '1009', 'ABC131', 'CHAVE009', 'PROTO009', CONVERT(DATETIME, '2024-07-09', 120), '<xml>...</xml>'),
('12345678000190', '1010', 'ABC132', 'CHAVE010', 'PROTO010', CONVERT(DATETIME, '2024-07-10', 120), '<xml>...</xml>'),
('12345678000189', '1011', 'ABC133', 'CHAVE011', 'PROTO011', CONVERT(DATETIME, '2024-07-11', 120), '<xml>...</xml>');


INSERT INTO dbo.NFCe (serie, numero_documento, codigo_numerico, chave, tag_id, protocolo, recebido_em, xml_autorizado) VALUES
('001', '2001', 'XYZ001', 'CHAVE1001', 'TAG001', 'PROTO1001', CONVERT(DATETIME, '2024-06-01', 120), '<xml>...</xml>'),
('001', '2002', 'XYZ002', 'CHAVE1002', 'TAG002', 'PROTO1002', CONVERT(DATETIME, '2024-06-02', 120), '<xml>...</xml>'),
('001', '2003', 'XYZ003', 'CHAVE1003', 'TAG003', 'PROTO1003', CONVERT(DATETIME, '2024-06-03', 120), '<xml>...</xml>'),
('001', '2004', 'XYZ004', 'CHAVE1004', 'TAG004', 'PROTO1004', CONVERT(DATETIME, '2024-06-04', 120), '<xml>...</xml>'),
('001', '2005', 'XYZ005', 'CHAVE1005', 'TAG005', 'PROTO1005', CONVERT(DATETIME, '2024-06-05', 120), '<xml>...</xml>'),
('001', '2006', 'XYZ006', 'CHAVE1006', 'TAG006', 'PROTO1006', CONVERT(DATETIME, '2024-06-06', 120), '<xml>...</xml>'),
('001', '2007', 'XYZ007', 'CHAVE1007', 'TAG007', 'PROTO1007', CONVERT(DATETIME, '2024-06-07', 120), '<xml>...</xml>'),
('001', '2008', 'XYZ008', 'CHAVE1008', 'TAG008', 'PROTO1008', CONVERT(DATETIME, '2024-06-08', 120), '<xml>...</xml>'),
('001', '2009', 'XYZ009', 'CHAVE1009', 'TAG009', 'PROTO1009', CONVERT(DATETIME, '2024-06-09', 120), '<xml>...</xml>'),
('001', '2010', 'XYZ010', 'CHAVE1010', 'TAG010', 'PROTO1010', CONVERT(DATETIME, '2024-06-10', 120), '<xml>...</xml>');

INSERT INTO dbo.Notas (cnpj_fornecedor, uid_fornecedor, numero_documento, serie, chave, xml_final) VALUES
('12345678000199', 'Fornecedor 1', '3001', '001', 'CHAVE5001', '<xml>...</xml>'),
('12345678000198', 'Fornecedor 2', '3002', '001', 'CHAVE5002', '<xml>...</xml>'),
('12345678000197', 'Fornecedor 3', '3003', '001', 'CHAVE5003', '<xml>...</xml>'),
('12345678000196', 'Fornecedor 4', '3004', '001', 'CHAVE5004', '<xml>...</xml>'),
('12345678000195', 'Fornecedor 5', '3005', '001', 'CHAVE5005', '<xml>...</xml>'),
('12345678000194', 'Fornecedor 6', '3006', '001', 'CHAVE5006', '<xml>...</xml>'),
('12345678000193', 'Fornecedor 7', '3007', '001', 'CHAVE5007', '<xml>...</xml>'),
('12345678000192', 'Fornecedor 8', '3008', '001', 'CHAVE5008', '<xml>...</xml>'),
('12345678000191', 'Fornecedor 9', '3009', '001', 'CHAVE5009', '<xml>...</xml>');

