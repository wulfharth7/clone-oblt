import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import SearchModule from './Components/Search/SearchModule';
import JourneyResults from './Components/Journey/JourneyResults';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<SearchModule />} />
        <Route
  path="/seferler/:routeString/:departureDate"
  element={<JourneyResults />}
/>

      </Routes>
    </Router>
  );
}

export default App;
