import { useState } from 'react';

const App = () => {
  const [taskList, setTaskList] = useState<string[]>([]);
  const [newTask, setNewTask] = useState<string>('');

  // Handle new task input change
  const handleChange = (e) => {
    setNewTask(e.target.value);
  };

  // Handle new task creation, by adding it into the list
  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!newTask) return;
    setTaskList([...taskList, newTask]);
    setNewTask('');
  }

  return (
    <>
      <div className="header">
        <h1>WhatToDo</h1>
      </div>
      <div className="container">
        <form>
          <input
            value={newTask}
            placeholder="New task"
            onChange={handleChange}
          />
          <button type="submit" onClick={handleSubmit}>
            Add new task
          </button>
        </form>
        <ul>
          {taskList.map((task) => (
            <li>{task}</li>
          ))}
        </ul>
      </div>
    </>
  );
};

export default App;

/* Erillaisia tyylejä määritellä React komponentteja / funktioita
  function App() {}
  const App = () => {}
  export default function App() {}
*/
