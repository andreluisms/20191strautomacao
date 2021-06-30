
USE `str_db`;

-- Copiando dados para a tabela str_db.organizacao: ~4 rows (aproximadamente)
INSERT INTO `organizacao` (`id`, `cnpj`, `razaoSocial`) VALUES
	(1, '08735240000146', 'FUNDAÇÃO UNIVERSIDADE FEDERAL DE SERGIPE'),
	(2, '57838165000154', 'UNIVERSIDADE TIRADENTES - UNIT'),
	(3, '30056954000187', 'MINISTÉRIO PÚBLICO DE SERGIPE'),
	(4, '50618535000107', 'PREFEITURA MUNICIPAL DE ARACAJU');
	
-- Copiando dados para a tabela str_db.bloco: ~13 rows (aproximadamente)
INSERT INTO `bloco` (`id`, `organizacao`, `titulo`) VALUES
	(1, 1, 'Bloco A'),
	(2, 1, 'Bloco B'),
	(3, 1, 'Bloco C'),
	(4, 1, 'Bloco D'),
	(5, 1, 'Bloco E'),
	(6, 2, 'Bloco SI'),
	(7, 2, 'Bloco ADM'),
	(8, 2, 'Bloco SAUDE'),
	(9, 2, 'Bloco BIO'),
	(10, 3, 'Bloco X'),
	(11, 3, 'Bloco Y'),
	(12, 3, 'Bloco Z'),
	(13, 4, 'Bloco Unico');
	
-- Copiando dados para a tabela str_db.sala: ~13 rows (aproximadamente)
INSERT INTO `sala` (`id`, `titulo`, `bloco`) VALUES
	(1, 'Sala 01', 1),
	(2, 'Sala 02', 2),
	(3, 'Sala 03', 3),
	(4, 'Sala 04', 4),
	(5, 'Sala 05', 5),
	(6, 'Sala 06', 6),
	(7, 'Sala 07', 7),
	(8, 'Sala 08', 8),
	(9, 'Sala 09', 9),
	(10, 'Sala 10', 10),
	(11, 'Sala 11', 11),
	(12, 'Sala 12', 12),
	(13, 'Sala 13', 13),
	(14, 'Sala 106', 4);
	
-- Copiando dados para a tabela str_db.tipohardware: ~3 rows (aproximadamente)
INSERT INTO `tipohardware` (`id`, `descricao`) VALUES
	(1, 'MODULO ATUADOR'),
	(2, 'CONTROLADOR DE SALA'),
	(3, 'MODULO DE SENSORIAMENTO');
		
	
	-- Copiando dados para a tabela str_db.hardwaredesala: ~39 rows (aproximadamente)
INSERT INTO `hardwaredesala` (`id`, `mac`,`ip`, `sala`, `tipoHardware`,`uuid`) VALUES
	(1, '00:E0:4C:27:56:96','192.168.15.233',  1, 1,NULL),
	(2, '00:E0:4C:5A:D1:DD', '192.168.15.233', 1, 2,NULL),
	(3, '00:E0:4C:50:B0:3E','192.168.15.233',  2, 1,NULL),
	(4, '00:E0:4C:50:B0:3E','192.168.15.233', 2, 2,NULL),
	(5, '00:E0:4C:66:82:0A', '192.168.15.233', 3, 1,NULL),
	(6, '00:E0:4C:59:A4:20','192.168.15.233',  3, 2,NULL),
	(7, '00:E0:4C:15:F6:A0', '192.168.15.233', 4, 1,NULL),
	(8, '00:E0:4C:1B:B5:35', '192.168.15.233', 4, 2,NULL),
	(9, '00:E0:4C:4E:A9:64', '192.168.15.233', 5, 1,NULL),
	(10, '00:E0:4C:04:B6:9E','192.168.15.233',  5, 2,NULL),
	(11, '00:E0:4C:3A:E1:F0','192.168.15.233',  6, 1,NULL),
	(12, '00:E0:4C:46:B6:13', '192.168.15.233', 6, 2,NULL),
	(13, '00:E0:4C:15:AA:D1', '192.168.15.233', 7, 1,NULL),
	(14, '00:E0:4C:71:5B:0B', '192.168.15.233', 7, 2,NULL),
	(15, '00:E0:4C:3B:3F:9F','192.168.15.233',  8, 1,NULL),
	(16, '00:E0:4C:31:19:0B', '192.168.15.233', 8, 2,NULL),
	(17, '00:E0:4C:5A:D1:DD', '192.168.15.233', 1, 3,NULL),
	(18, '00:E0:4C:32:A1:93', '192.168.15.233', 2, 3,NULL),
	(19, '00:E0:4C:04:56:7B', '192.168.15.233', 3, 3,NULL),
	(20, '00:E0:4C:32:F0:A6', '192.168.15.233', 4, 3,NULL),
	(21, '00:E0:4C:43:7C:3E', '192.168.15.233', 5, 3,NULL),
	(22, '00:E0:4C:7B:D9:3E', '192.168.15.233', 6, 3,NULL),
	(23, '00:E0:4C:7A:3A:A4', '192.168.15.233', 7, 3,NULL),
	(24, '00:E0:4C:5B:DD:23', '192.168.15.233', 8, 3,NULL),
	(25, '00:E0:4C:08:72:05', '192.168.15.233', 9, 1,NULL),
	(26, '00:E0:4C:33:61:87', '192.168.15.233', 9, 2,NULL),
	(27, '00:E0:4C:39:C1:CC', '192.168.15.233', 9, 3,NULL),
	(28, '00:E0:4C:44:06:E4', '192.168.15.233', 10, 1,NULL),
	(29, '00:E0:4C:1B:38:1F', '192.168.15.233', 10, 2,NULL),
	(30, '00:E0:4C:1C:C2:42', '192.168.15.233', 10, 3,NULL),
	(31, '00:E0:4C:26:F7:80', '192.168.15.233', 11, 1,NULL),
	(32, '00:E0:4C:7B:75:7F', '192.168.15.233', 11, 2,NULL),
	(33, '00:E0:4C:41:AA:BD', '192.168.15.233', 11, 3,NULL),
	(34, '00:E0:4C:7A:16:3A', '192.168.15.233', 12, 1,NULL),
	(35, '00:E0:4C:4B:8A:65', '192.168.15.233', 12, 2,NULL),
	(36, '00:E0:4C:43:85:71', '192.168.15.233', 12, 3,NULL),
	(37, '00:E0:4C:5F:B4:08', '192.168.15.233', 13, 1,NULL),
	(38, '00:E0:4C:21:50:B0', '192.168.15.233', 13, 2,NULL),
	(39, '00:E0:4C:28:5F:41', '192.168.15.233', 13, 3,NULL);
	
-- Copiando dados para a tabela str_db.tipousuario: ~2 rows (aproximadamente)
INSERT INTO `tipousuario` (`id`, `descricao`) VALUES
	(1, 'ADMIN'),
	(2, 'GESTOR'),
	(3, 'CLIENTE');
	
-- Copiando dados para a tabela str_db.usuario: ~2 rows (aproximadamente)
INSERT INTO `usuario` (`id`, `cpf`, `nome`, `dataNascimento`, `senha`, `tipoUsuario`) VALUES
	(1, '42112664204', 'Lívia Benedita Rebeca Araújo', '1997-08-15', '60BFAA61E12B4FD3DAD35586B11387689E35645279C6103495F019AAA0C1FCF3', 2),
	(2, '57377766387', 'Rafael Kevin Teixeira', '1996-07-22', '60BFAA61E12B4FD3DAD35586B11387689E35645279C6103495F019AAA0C1FCF3', 2),
	(3, '07852892590', 'Igor bruno dos santos nascimento', '1996-07-22', '60BFAA61E12B4FD3DAD35586B11387689E35645279C6103495F019AAA0C1FCF3', 2),
	(4, '07334824571', 'Abraao Alves', '1998-06-06', '60BFAA61E12B4FD3DAD35586B11387689E35645279C6103495F019AAA0C1FCF3', 1);

	
-- Copiando dados para a tabela str_db.usuarioorganizacao: ~0 rows (aproximadamente)
INSERT INTO `usuarioorganizacao` (`id`, `organizacao`, `usuario`) VALUES
	(1, 1, 1),
	(3, 1, 3),
	(2, 1, 2),
	(4, 1, 4);
	
	
-- Copiando dados para a tabela str_db.horariosala: ~0 rows (aproximadamente)
INSERT INTO `horariosala` (`id`, `data`, `horarioInicio`,`horarioFim`,`objetivo`,`usuario`,`sala`,`planejamento`) VALUES
							(1, '2020-08-24', '07:00','09:00','Palestra sobre Engenharia de Software',1,4,null),
							(2, '2020-09-20', '07:00','09:00','Palestra sobre Engenharia de Software',2,4,null);
			
-- Copiando dados para a tabela str_db.planejamento: ~0 rows (aproximadamente)
INSERT INTO `planejamento` (`id`, `dataInicio`, `dataFim`,`horarioInicio`,`horarioFim`,`diaSemana`,`objetivo`,`usuario`,`sala`) VALUES
	(1, '2020-08-24', '2020-12-24','07:00','09:00','SEG','Planejamento de LFT',1,5),
	(2, '2020-08-24', '2020-12-24','07:00','09:00','QUA','Planejamento de LFT',1,5),
	(3, '2020-08-24', '2020-12-24','07:00','09:00','SEX','Planejamento de LFT',1,5);
	
-- Copiando dados para a tabela str_db.salaparticular: ~0 rows (aproximadamente)
INSERT INTO `salaparticular` (`id`, `usuario`,`sala`) VALUES 
				(1,1,1),
				(2,2,2),
				(3,2,4),
				(4,2,1),
				(5,1,3);

INSERT INTO `equipamento` (`id`, `modelo`,`marca`,`descricao`,`sala`,`hardwareDeSala`) VALUES 
				(1,"SAMSUNG","220TT","Condicionador SAMSUNG 110V classe de consumo E",1,1),
				(2,"ELGIN","SGV330","Condicionador ELGIN 110V classe de consumo G",2,3),
				(3,"LG","EXTENDER","Condicionador LG 110V classe de consumo G",3,3);

INSERT INTO `monitoramento` (`id`, `estado`,`equipamento`) VALUES
	(1, 0, 1),
	(2, 0, 2),
	(3, 0, 3);

INSERT INTO `operacao` (`id`, `titulo`,`descricao`) VALUES 
				(1,"Ligar","Liga Dispositivo"),
				(2,"Desligar","Desliga Dispotivo");	
				
INSERT INTO `codigoinfravermelho` (`id`, `equipamento`, `operacao`, `codigo`) VALUES
	(1, 1, 2, '460,264,184,256,182,584,184,422,184,256,182,584,186,258,182,256,182,256,184,586,184,584,186,420,184,584,186,280,158,282,160,748,186,264,188'),
	(2, 2, 2, '8412,4232,496,556,548,1484,580,1568,524,536,528,1572,524,544,528,1580,520,540,496,556,524,1568,520,536,500,560,552,1548,552,1552,552,1552,524,540,496,556,500,552,500,560,524,516,520,568,496,572,524,552,524,536,496,556,520,512,520,560,500,564,496,568,500,572,496,576,496,544,516,556,496,560,496,540,516,568,496,568,552,1528,544,552,500,560,496,556,520,512,524,536,516,568,500,544,516,576,524,1556,548,1548,516,560,496,556,524,536,496,544,516,572,500,548,520,576,496,564,492,560,496,556,500,560,496,568,496,544,520,572,500,576,496,564,496,556,496,556,548,1520,576,1544,524,544,496,572,496,576,500,560,500,552,500,556,496,564,520,540,496,572,496,572,496,576,496,512,576,508,516,560,524,1568,552,1520,524,564,500,548,544,552,520,516,572,1516,572,1512,524,536,548,1548,548,1576,520,548,496,580,496,564,544,1516,576,1512,572,1520,524,540,520,568,520,548,500,556,540,540,548,1512,524,532,520,564,524,1548,544,544,496,572,500,552,520,564,496,528,552,1560,528,1544,520,540,548,1556,520,544,548,1584,520,508,492'),
	(3, 1, 1, '9076,4438,612,522,610,522,610,1640,610,524,610,522,610,522,608,524,584,548,584,1666,586,1666,586,548,584,1666,586,1666,586,1666,586,1668,584,1668,586,548,584,548,584,548,584,1666,586,548,584,548,586,548,586,546,586,1666,588,1664,586,1666,588,546,586,1666,586,1666,586,1666,588,1664,586,39900,9028,2226,584');