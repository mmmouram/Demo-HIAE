import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { obterHistoricoPaciente } from '../services/PacienteService';
import './HistoricoPacientePage.css';

function HistoricoPaciente() {
  const { idPaciente } = useParams();
  const [historicos, setHistoricos] = useState([]);
  const [erro, setErro] = useState('');
  const [carregando, setCarregando] = useState(false);
  const navigate = useNavigate();
  const idMedico = 1; // Exemplo estático

  const carregarHistorico = async () => {
    setCarregando(true);
    setErro('');
    try {
      const dados = await obterHistoricoPaciente(idPaciente, idMedico);
      if (Array.isArray(dados) && dados.length === 0) {
        setErro('Não há registros disponíveis para este paciente.');
      } else {
        setHistoricos(dados);
      }
    } catch (error) {
      if (error.response && error.response.status === 403) {
        setErro('Acesso negado. O médico não tem permissão para acessar os dados deste paciente.');
      } else {
        setErro('Falha ao carregar o histórico. Verifique sua conexão.');
      }
    } finally {
      setCarregando(false);
    }
  };

  useEffect(() => {
    carregarHistorico();
  }, [idPaciente]);

  return (
    <div className="container-historico">
      <h1>Histórico do Paciente</h1>
      {carregando && <p>Carregando...</p>}
      {erro && (
        <div className="erro">
          <p>{erro}</p>
          <div className="botoes">
            <button onClick={carregarHistorico}>Tentar novamente</button>
            <button onClick={() => navigate('/')} >Voltar à lista</button>
          </div>
        </div>
      )}
      {!carregando && !erro && (
        <>
          {historicos.length === 0 ? (
            <p>Nenhum registro encontrado.</p>
          ) : (
            <ul className="lista-historicos">
              {historicos.map((historico) => (
                <li key={historico.id} className="item-historico">
                  <p><strong>Data:</strong> {new Date(historico.dataRegistro).toLocaleString()}</p>
                  <p><strong>Tipo:</strong> {historico.tipo}</p>
                  <p><strong>Descrição:</strong> {historico.descricao}</p>
                </li>
              ))}
            </ul>
          )}
          <button className="btn-voltar" onClick={() => navigate('/')}>Voltar à lista</button>
        </>
      )}
    </div>
  );
}

export default HistoricoPaciente;