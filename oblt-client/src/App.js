import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import SearchModule from './SearchModule';
import JourneyResults from './JourneyResults';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<SearchModule />} />
        <Route path="/journey-results" element={<JourneyResults />} />
      </Routes>
    </Router>
  );
}

export default App;
