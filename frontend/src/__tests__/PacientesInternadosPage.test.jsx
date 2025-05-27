import React from 'react';
import { render, screen, waitFor, fireEvent } from '@testing-library/react';
import ListaPacientesInternados from '../pages/PacientesInternadosPage';
import { obterPacientesInternados } from '../services/PacienteService';
import { BrowserRouter } from 'react-router-dom';

jest.mock('../services/PacienteService');

const mockedNavigate = jest.fn();
jest.mock('react-router-dom', () => {
  const actual = jest.requireActual('react-router-dom');
  return {
    ...actual,
    useNavigate: () => mockedNavigate,
  };
});


describe('ListaPacientesInternados', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  test('deve renderizar a lista de pacientes internados', async () => {
    const pacientes = [
      { id: 1, nome: 'Paciente A' },
      { id: 2, nome: 'Paciente B' }
    ];
    obterPacientesInternados.mockResolvedValue(pacientes);

    render(
      <BrowserRouter>
        <ListaPacientesInternados />
      </BrowserRouter>
    );

    // Componente inicia exibindo mensagem de carregamento
    expect(screen.getByText(/Carregando.../i)).toBeInTheDocument();

    // Aguarda que o carregamento seja finalizado
    await waitFor(() => {
      expect(screen.queryByText(/Carregando.../i)).not.toBeInTheDocument();
    });

    // Verifica se os pacientes são exibidos
    pacientes.forEach(paciente => {
      expect(screen.getByText(paciente.nome)).toBeInTheDocument();
    });

    // Simula a seleção de um paciente
    fireEvent.click(screen.getByText('Paciente A'));
    expect(mockedNavigate).toHaveBeenCalledWith(`/historico/1`);
  });

  test('deve exibir mensagem de erro em caso de falha ao carregar pacientes e permitir retry', async () => {
    obterPacientesInternados.mockRejectedValue(new Error('Erro de conexão'));

    render(
      <BrowserRouter>
        <ListaPacientesInternados />
      </BrowserRouter>
    );

    await waitFor(() => {
      expect(screen.getByText(/Falha ao carregar pacientes. Verifique sua conexão./i)).toBeInTheDocument();
    });

    // Verifica se o botão de retry está presente
    const retryButton = screen.getByText(/Tentar novamente/i);
    expect(retryButton).toBeInTheDocument();

    // Simula uma tentativa de recarregar com sucesso
    const pacientes = [{ id: 3, nome: 'Paciente C' }];
    obterPacientesInternados.mockResolvedValueOnce(pacientes);
    fireEvent.click(retryButton);

    await waitFor(() => {
      expect(screen.getByText('Paciente C')).toBeInTheDocument();
    });
  });

  test('deve exibir mensagem quando não há pacientes internados', async () => {
    obterPacientesInternados.mockResolvedValue([]);

    render(
      <BrowserRouter>
        <ListaPacientesInternados />
      </BrowserRouter>
    );

    await waitFor(() => {
      expect(screen.getByText(/Não há pacientes internados./i)).toBeInTheDocument();
    });
  });
});
