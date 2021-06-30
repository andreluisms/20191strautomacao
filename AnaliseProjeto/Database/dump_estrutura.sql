
CREATE DATABASE IF NOT EXISTS `str_db`; 
USE `str_db`;

-- Copiando estrutura para tabela str_db.organizacao
CREATE TABLE IF NOT EXISTS `organizacao` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `cnpj` varchar(15) NOT NULL,
  `razaoSocial` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=UTF8;


-- Copiando estrutura para tabela str_db.bloco
CREATE TABLE IF NOT EXISTS `bloco` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `organizacao` int unsigned NOT NULL,
  `titulo` varchar(100) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Bloco_Organizacao1_idx` (`organizacao`),
  CONSTRAINT `fk_Bloco_Organizacao1` FOREIGN KEY (`organizacao`) REFERENCES `organizacao` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;



-- Copiando estrutura para tabela str_db.sala
CREATE TABLE IF NOT EXISTS `sala` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `titulo` varchar(100) NOT NULL,
  `bloco` int unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Sala_Bloco1_idx` (`bloco`),
  CONSTRAINT `fk_Sala_Bloco1` FOREIGN KEY (`Bloco`) REFERENCES `bloco` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;
	
-- Copiando estrutura para tabela str_db.tipohardware
CREATE TABLE IF NOT EXISTS `tipohardware` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `descricao` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=UTF8;


-- Copiando estrutura para tabela str_db.hardwaredesala
CREATE TABLE IF NOT EXISTS `hardwaredesala` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `mac` varchar(45) NOT NULL,
  `Sala` int unsigned NOT NULL,
  `tipoHardware` int unsigned NOT NULL,
  `ip` VARCHAR(15) NULL,
  `uuid` VARCHAR(75) NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Hardware_Sala1_idx` (`sala`),
  KEY `fk_HardwareDeSala_TipoHardware1_idx` (`tipoHardware`),
  CONSTRAINT `fk_Hardware_Sala1` FOREIGN KEY (`Sala`) REFERENCES `sala` (`id`),
  CONSTRAINT `fk_HardwareDeSala_TipoHardware1` FOREIGN KEY (`tipoHardware`) REFERENCES `tipohardware` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8;

-- Copiando estrutura para tabela str_db.tipousuario
CREATE TABLE IF NOT EXISTS `tipousuario` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `descricao` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- Copiando estrutura para tabela str_db.usuario
CREATE TABLE IF NOT EXISTS `usuario` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `cpf` varchar(11) NOT NULL,
  `nome` varchar(45) NOT NULL,
  `dataNascimento` date DEFAULT NULL,
  `senha` varchar(100) NOT NULL,
  `tipoUsuario` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `Cpf_UNIQUE` (`cpf`),
  KEY `fk_Usuario_TipoUsuario1_idx` (`tipoUsuario`),
  CONSTRAINT `fk_Usuario_TipoUsuario1` FOREIGN KEY (`tipoUsuario`) REFERENCES `tipousuario` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;


-- Copiando estrutura para tabela str_db.usuarioorganizacao
CREATE TABLE IF NOT EXISTS `usuarioorganizacao` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `organizacao` int unsigned NOT NULL,
  `usuario` int unsigned NOT NULL,
  PRIMARY KEY (`id`,`Organizacao`,`usuario`),
  KEY `fk_Organizacao_has_Usuario_Usuario1_idx` (`usuario`),
  KEY `fk_Organizacao_has_Usuario_Organizacao1_idx` (`organizacao`),
  CONSTRAINT `fk_Organizacao_has_Usuario_Organizacao1` FOREIGN KEY (`organizacao`) REFERENCES `organizacao` (`id`),
  CONSTRAINT `fk_Organizacao_has_Usuario_Usuario1` FOREIGN KEY (`usuario`) REFERENCES `usuario` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Copiando estrutura para tabela str_db.planejamento
CREATE TABLE IF NOT EXISTS `planejamento` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `dataInicio` date NOT NULL,
  `dataFim` date NOT NULL,
  `horarioInicio` time NOT NULL,
  `horarioFim` time NOT NULL,
  `diaSemana` enum('SEG','TER','QUA','QUI','SEX','SAB','DOM') NOT NULL,
  `objetivo` varchar(500) NOT NULL,
  `usuario` int unsigned NOT NULL,
  `sala` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Planejamento_Usuario1_idx` (`usuario`),
  KEY `fk_Planejamento_Sala1_idx` (`sala`),
  CONSTRAINT `fk_Planejamento_Sala1` FOREIGN KEY (`sala`) REFERENCES `sala` (`id`),
  CONSTRAINT `fk_Planejamento_Usuario1` FOREIGN KEY (`usuario`) REFERENCES `usuario` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



-- Copiando estrutura para tabela str_db.horariosala
CREATE TABLE IF NOT EXISTS `horariosala` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `data` datetime NOT NULL,
  `horarioInicio` time NOT NULL,
  `horarioFim` time NOT NULL,
  `situacao` ENUM('PENDENTE', 'APROVADA','REPROVADA', 'CANCELADA') NOT NULL DEFAULT 'APROVADA',
  `objetivo` varchar(500) NOT NULL,
  `usuario` int unsigned NOT NULL,
  `sala` int unsigned NOT NULL,
  `planejamento` int unsigned NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_HorarioSala_Usuario1_idx` (`usuario`),
  KEY `fk_HorarioSala_Sala1_idx` (`sala`),
  KEY `fk_HorarioSala_Planejamento1_idx` (`planejamento`),
  CONSTRAINT `fk_HorarioSala_Sala1` FOREIGN KEY (`sala`) REFERENCES `sala` (`id`),
  CONSTRAINT `fk_HorarioSala_Usuario1` FOREIGN KEY (`usuario`) REFERENCES `usuario` (`id`),
  CONSTRAINT `fk_HorarioSala_Planejamento1` FOREIGN KEY (`planejamento`) REFERENCES `planejamento` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;    


-- Copiando estrutura para tabela str_db.salaparticular
CREATE TABLE IF NOT EXISTS `salaparticular` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `usuario` int unsigned NOT NULL,
  `sala` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_MinhaSala_Usuario1_idx` (`usuario`),
  KEY `fk_MinhaSala_Sala1_idx` (`sala`),
  CONSTRAINT `fk_MinhaSala_Sala1` FOREIGN KEY (`sala`) REFERENCES `sala` (`id`),
  CONSTRAINT `fk_MinhaSala_Usuario1` FOREIGN KEY (`usuario`) REFERENCES `usuario` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE IF NOT EXISTS `equipamento` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `modelo` VARCHAR(200) NOT NULL,
  `marca` VARCHAR(100) NOT NULL,
  `descricao` VARCHAR(1000) NULL DEFAULT NULL,
  `sala` INT(10) UNSIGNED NOT NULL,
  `tipoEquipamento` ENUM('CONDICIONADOR', 'LUZES') DEFAULT ('CONDICIONADOR') NOT NULL,
  `hardwareDeSala` INT UNSIGNED NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Equipamento_Sala1_idx` (`sala`),
  CONSTRAINT `fk_Equipamento_Sala1` FOREIGN KEY (`sala`) REFERENCES `str_db`.`sala` (`id`),
  KEY `fk_Equipamento_HardwareDeSala_idx` (`hardwareDeSala`),
  CONSTRAINT `fk_Equipamento_HardwareDeSala` FOREIGN KEY (`hardwareDeSala`) REFERENCES `str_db`.`hardwaredesala` (`id`)
) ENGINE = InnoDB DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `monitoramento` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `estado` TINYINT(4) NOT NULL,
  `equipamento` INT(11)  NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_Equipamento_Id` FOREIGN KEY (`equipamento`) REFERENCES `Equipamento` (`id`)
) ENGINE = InnoDB DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `operacao` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `Titulo` VARCHAR(50) NOT NULL,
  `Descricao` VARCHAR(200) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB DEFAULT CHARACTER SET = UTF8;

CREATE TABLE IF NOT EXISTS `codigoInfravermelho` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `equipamento` INT(11) NOT NULL,
  `operacao` INT(11) NOT NULL,
  `codigo` TEXT(65535) NOT NULL COLLATE 'utf8_unicode_ci',
  PRIMARY KEY (`id`),
  KEY `fk_CodigoInfravermelho_Equipamento1_idx` (`equipamento` ASC) VISIBLE,
  KEY `fk_CodigoInfravermelho_Operacao1_idx` (`operacao` ASC) VISIBLE,
  CONSTRAINT `fk_CodigoInfravermelho_Equipamento1` FOREIGN KEY (`equipamento`) REFERENCES `str_db`.`equipamento` (`id`),
  CONSTRAINT `fk_CodigoInfravermelho_Operacao1` FOREIGN KEY (`operacao`) REFERENCES `str_db`.`operacao` (`id`)
)ENGINE = InnoDB DEFAULT CHARACTER SET = utf8;






