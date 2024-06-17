import React, { useState, useEffect } from 'react';

function App() {
  const [count, setCount] = useState(0);

  useEffect(() => {
    console.log('useEffect [] triggered');
  }, []);

  useEffect(() => {
    console.log('useEffect Count state updated');
  }, [count]);
  
  useEffect(() => {
    console.log('useEffect triggered');
  });




  const handleClick = () => {
    console.log('Button clicked');
    setCount(count + 1);
  };

  return (
    <div className="App">
      <header className="App-header">
        <h1>React Hooks Example</h1>
        <button onClick={handleClick}>Click Me</button>
        <p>Count: {count}</p>
      </header>
    </div>
  );
}

export default App;
