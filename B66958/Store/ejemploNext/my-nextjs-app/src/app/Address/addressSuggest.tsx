import React, { useState } from 'react';
import "../css/address.css"

const AddressModal = ({ onSelect }: { onSelect: (address: string) => void }) => {
  const [query, setQuery] = useState('');
  const [suggestions, setSuggestions] = useState([]);

  const fetchSuggestions = async (query: string) => {
    if (query.length < 3) return;
    const response = await fetch(`https://nominatim.openstreetmap.org/search?format=json&countrycodes=cr&q=${query}`);
    const data = await response.json();
    setSuggestions(data);
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    setQuery(value);
    fetchSuggestions(value);
  };

  const handleSuggestionClick = (suggestion: any) => {
    onSelect(suggestion.display_name);
  };

  return (
    <div className="modal">
      <div className="modal-content">
        <span className="close" onClick={() => onSelect('')}>&times;</span>
        <input
          type="text"
          className="form-control"
          value={query}
          placeholder="Buscar dirección"
          onChange={handleInputChange}
        />
        {suggestions.length > 0 && (
          <ul className="suggestions-list">
            {suggestions.map((suggestion: any, index: number) => (
              <li key={index} onClick={() => handleSuggestionClick(suggestion)}>
                {suggestion.display_name}
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
};

export default AddressModal;
