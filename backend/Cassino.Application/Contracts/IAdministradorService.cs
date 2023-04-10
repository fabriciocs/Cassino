﻿using Cassino.Application.Dtos.V1.Administrador;
using Cassino.Application.Dtos.V1.Base;

namespace Cassino.Application.Contracts;

public interface IAdministradorService
{
    Task<PagedDto<AdministradorDto>> Buscar(BuscarAdministradorDto dto);
    Task<AdministradorDto?> Adicionar(AdicionarAdministradorDto dto);
    Task<AdministradorDto?> Alterar(int id, AlterarAdministradorDto dto);
    Task<AdministradorDto?> ObterPorId(int id);
    Task<AdministradorDto?> ObterPorEmail(string email);
    Task Desaticar(int id);
    Task Reativar(int id);
}