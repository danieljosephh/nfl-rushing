import logo from './logo.svg';
import './App.css';
import { api } from './_common/_api/api.helpers';
import { RushingStatsTable } from './RushingStatsTable';

function App() {
  return (
    <div className="App">
      <RushingStatsTable />
    </div>
  );
}

export default App;
