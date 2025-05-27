import axios from 'axios';

const API_BASE_URL = '/api/PacienteInternacao';

export async function obterPacientesInternados(idMedico) {
    const response = await axios.get(API_BASE_URL, { params: { idMedico } });
    return response.data;
}

export async function obterHistoricoPaciente(idPaciente, idMedico) {
    const response = await axios.get(`${API_BASE_URL}/${idPaciente}/historico`, { params: { idMedico } });
    return response.data;
}