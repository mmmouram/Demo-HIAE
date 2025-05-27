import React from 'react';
import { render, screen } from '@testing-library/react';
import App from '../App';
import { MemoryRouter } from 'react-router-dom';


describe('App Routing', () => {
  test('deve renderizar a página de pacientes internados na rota raiz "/"', () => {
    render(
      <MemoryRouter initialEntries={['/']}>
        <App />
      </MemoryRouter>
    );

    expect(screen.getByText(/Pacientes em Internação/i)).toBeInTheDocument();
  });

  test('deve renderizar a página de histórico do paciente na rota "/historico/:idPaciente"', () => {
    render(
      <MemoryRouter initialEntries={['/historico/1']}>
        <App />
      </MemoryRouter>
    );

    expect(screen.getByText(/Histórico do Paciente/i)).toBeInTheDocument();
  });
});
