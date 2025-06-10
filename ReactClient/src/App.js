import React, { useEffect, useState } from 'react';

function App() {
  const [message, setMessage] = useState('Loading...');

  useEffect(() => {
    fetch('/api/WeatherForecast')
      .then(res => res.ok ? res.json() : Promise.reject('Error'))
      .then(data => setMessage(JSON.stringify(data)))
      .catch(() => setMessage('Failed to load from API'));
  }, []);

  return <div>{message}</div>;
}

export default App;
