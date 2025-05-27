import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { obterPacientesInternados } from '../services/PacienteService';
import './PacientesInternadosPage.css';

function ListaPacientesInternados() {
  const [pacientes, setPacientes] = useState([]);
  const [erro, setErro] = useState('');
  const [carregando, setCarregando] = useState(false);
  const navigate = useNavigate();
  const idMedico = 1; // Exemplo estático, em cenário real seria retirado do token

  const carregarPacientes = async () => {
    setCarregando(true);
    setErro('');
    try {
      const dados = await obterPacientesInternados(idMedico);
      setPacientes(dados);
    } catch (error) {
      setErro('Falha ao carregar pacientes. Verifique sua conexão.');
    } finally {
      setCarregando(false);
    }
  };

  useEffect(() => {
    carregarPacientes();
  }, []);

  const selecionarPaciente = (idPaciente) => {
    navigate(`/historico/${idPaciente}`);
  };

  return (
    <div className="container-pacientes">
      <h1>Pacientes em Internação</h1>
      {carregando && <p>Carregando...</p>}
      {erro && (
        <div className="erro">
          <p>{erro}</p>
          <button onClick={carregarPacientes}>Tentar novamente</button>
        </div>
      )}
      {!carregando && !erro && (
        <>
          {pacientes.length === 0 ? (
            <p>Não há pacientes internados.</p>
          ) : (
            <ul className="lista-pacientes">
              {pacientes.map((paciente) => (
                <li
                  key={paciente.id}
                  className="item-paciente"
                  onClick={() => selecionarPaciente(paciente.id)}
                >
                  {paciente.nome}
                </li>
              ))}
            </ul>
          )}
        </>
      )}
    </div>
  );
}

export default ListaPacientesInternados;