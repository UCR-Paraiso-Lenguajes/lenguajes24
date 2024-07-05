import React, { useEffect, useState } from 'react';
import { useWebSocket } from '../../hooks/WebSocketContext';

interface CampaignMessage {
  id: number;
  title: string;
  content: string;
}

const Campaigns: React.FC = () => {
  const { messages } = useWebSocket();
  const [campaigns, setCampaigns] = useState<CampaignMessage[]>([]);
  const URLConection = process.env.NEXT_PUBLIC_API;
  useEffect(() => {
    fetch('${URLConection}/api/campaign')
      .then(response => response.json())
      .then(data => setCampaigns(data))
      .catch(error => console.error("Error fetching campaigns: ", error));
  }, []);

  useEffect(() => {
    setCampaigns(messages);
  }, [messages]);

  return (
    <div>
      <h1>Campaigns</h1>
      <ul>
        {campaigns.map((campaign) => (
          <li key={campaign.id}>
            <h2>{campaign.title}</h2>
            <div dangerouslySetInnerHTML={{ __html: campaign.content }}></div>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Campaigns;
