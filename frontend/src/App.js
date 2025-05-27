import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import ListaPacientesInternados from './pages/PacientesInternadosPage';
import HistoricoPaciente from './pages/HistoricoPacientePage';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<ListaPacientesInternados />} />
                <Route path="/historico/:idPaciente" element={<HistoricoPaciente />} />
            </Routes>
        </Router>
    );
}

export default App;