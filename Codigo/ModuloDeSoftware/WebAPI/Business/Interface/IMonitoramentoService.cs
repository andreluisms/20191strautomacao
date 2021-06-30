using Model;
using Model.ViewModel;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IMonitoramentoService
    {
        List<MonitoramentoModel> GetAll();
        MonitoramentoModel GetById(int id);
        MonitoramentoModel GetByIdEquipamento(int idEquipamento);
        bool Insert(MonitoramentoModel model);
        bool Update(MonitoramentoModel model);
        List<MonitoramentoModel> GetByIdSala(int idSala);
        bool MonitorarSala(int idUsuario, MonitoramentoViewModel monitoramento);
        bool MonitorarEquipamento(int idUsuario, MonitoramentoModel model);

    }
}
