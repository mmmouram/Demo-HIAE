import React from 'react';
import { render, screen, waitFor, fireEvent } from '@testing-library/react';
import HistoricoPaciente from '../pages/HistoricoPacientePage';
import { obterHistoricoPaciente } from '../services/PacienteService';
import { MemoryRouter, Route, Routes } from 'react-router-dom';

jest.mock('../services/PacienteService');

describe('HistoricoPaciente', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  const renderWithRouter = (initialEntries) => {
    return render(
      <MemoryRouter initialEntries={initialEntries}>
        <Routes>
          <Route path="/historico/:idPaciente" element={<HistoricoPaciente />} />
        </Routes>
      </MemoryRouter>
    );
  };

  test('deve exibir o histórico do paciente quando registros estão disponíveis', async () => {
    const historicos = [
      { id: 1, dataRegistro: new Date().toISOString(), tipo: 'Evolução', descricao: 'Evolução do paciente' },
      { id: 2, dataRegistro: new Date().toISOString(), tipo: 'Exame', descricao: 'Resultado de exame' }
    ];
    obterHistoricoPaciente.mockResolvedValue(historicos);

    renderWithRouter(['/historico/1']);

    // Verifica a mensagem de carregamento
    expect(screen.getByText(/Carregando.../i)).toBeInTheDocument();

    await waitFor(() => {
      expect(screen.queryByText(/Carregando.../i)).not.toBeInTheDocument();
    });

    // Verifica se os dados do histórico são exibidos
    historicos.forEach(hist => {
      expect(screen.getByText(new RegExp(hist.tipo, 'i'))).toBeInTheDocument();
      expect(screen.getByText(new RegExp(hist.descricao, 'i'))).toBeInTheDocument();
    });

    // Verifica se o botão de voltar à lista está presente
    expect(screen.getByText(/Voltar à lista/i)).toBeInTheDocument();
  });

  test('deve exibir mensagem quando não há registros disponíveis para o paciente', async () => {
    obterHistoricoPaciente.mockResolvedValue([]);

    renderWithRouter(['/historico/2']);

    await waitFor(() => {
      expect(screen.getByText(/Não há registros disponíveis para este paciente./i)).toBeInTheDocument();
    });
  });

  test('deve exibir mensagem de acesso negado para erro 403', async () => {
    const error403 = { response: { status: 403 } };
    obterHistoricoPaciente.mockRejectedValue(error403);

    renderWithRouter(['/historico/3']);

    await waitFor(() => {
      expect(screen.getByText(/Acesso negado. O médico não tem permissão para acessar os dados deste paciente./i)).toBeInTheDocument();
    });
  });

  test('deve exibir mensagem de falha ao carregar histórico em caso de erro de conexão e permitir retry', async () => {
    obterHistoricoPaciente.mockRejectedValue(new Error('Erro de conexão'));

    renderWithRouter(['/historico/4']);

    await waitFor(() => {
      expect(screen.getByText(/Falha ao carregar o histórico. Verifique sua conexão./i)).toBeInTheDocument();
    });

    const retryButton = screen.getByText(/Tentar novamente/i);
    expect(retryButton).toBeInTheDocument();

    // Simula uma tentativa de recarregar com sucesso
    const historicos = [
      { id: 3, dataRegistro: new Date().toISOString(), tipo: 'Tratamento', descricao: 'Detalhes de tratamento' }
    ];
    obterHistoricoPaciente.mockResolvedValueOnce(historicos);

    fireEvent.click(retryButton);

    await waitFor(() => {
      expect(screen.getByText(new RegExp(historicos[0].tipo, 'i'))).toBeInTheDocument();
      expect(screen.getByText(new RegExp(historicos[0].descricao, 'i'))).toBeInTheDocument();
    });
  });
});
