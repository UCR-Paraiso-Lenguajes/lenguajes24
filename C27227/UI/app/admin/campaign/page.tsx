"use client"
import React, { useState, useEffect } from 'react';
import { useWebSocket } from '../../hooks/WebSocketContext';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useRouter } from 'next/navigation';
import { decodeToken, checkTokenDate } from '../../hooks/jwtHooks';

interface CampaignMessage {
  id: number;
  title: string;
  content: string;
}

const CampaignPage: React.FC = () => {
  const { messages } = useWebSocket();
  const [campaigns, setCampaigns] = useState<CampaignMessage[]>([]);
  const [showModal, setShowModal] = useState(false);
  const [newCampaign, setNewCampaign] = useState({ title: '', content: '' });
  const [error, setError] = useState('');
  const URLConection = process.env.NEXT_PUBLIC_API;
  const router = useRouter();

  const checkToken = () => {
    const token = sessionStorage.getItem("sessionToken");
    if (token) {
      const decodedToken = decodeToken(token);
      const isTokenAlive = checkTokenDate(decodedToken?.exp);
      if (!isTokenAlive) {
        sessionStorage.removeItem("sessionToken");
        sessionStorage.removeItem("expiracyToken");
        router.push("/admin");
        return false;
      }
      return true;
    } else {
      router.push("/admin");
      return false;
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setNewCampaign({ ...newCampaign, [name]: value });
  };

  const handleSubmit = async () => {
    if (newCampaign.content.length > 5000) {
      setError('El contenido no puede exceder los 5000 caracteres');
      return;
    }
    const duplicateCampaign = campaigns.find(
      (campaign) => campaign.title === newCampaign.title
    );
    if (duplicateCampaign) {
      setError('Ya existe una campaña con el mismo título');
      return;
    }

    if (!checkToken()) return;

    const token = sessionStorage.getItem('sessionToken');
    try {
      const response = await fetch(`${URLConection}/api/campaign`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(newCampaign),
      });

      if (!response.ok) {
        throw new Error('Error creando la campaña');
      }
      await fetchCampaigns();

      setShowModal(false);
      setNewCampaign({ title: '', content: '' });
      setError('');
    } catch (error) {
      setError(error.message);
    }
  };

  const fetchCampaigns = async () => {
    if (!checkToken()) return;

    try {
      const token = sessionStorage.getItem('sessionToken');
      const response = await fetch(`${URLConection}/api/campaign`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        throw new Error('Error al obtener las campañas');
      }

      const data = await response.json();
      setCampaigns(data);
    } catch (error) {
      setError(error.message);
    }
  };

  const handleDelete = async (id: number) => {
    if (!checkToken()) return;

    const token = sessionStorage.getItem('sessionToken');
    try {
      const response = await fetch(`${URLConection}/api/campaign/${id}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        throw new Error('Error eliminando la campaña');
      }
      await fetchCampaigns();
    } catch (error) {
      setError(error.message);
    }
  };

  useEffect(() => {
    if (checkToken()) {
      fetchCampaigns();
    }
  }, [router]);

  return (
    <div className="container mt-5">
      <h1>Campaigns</h1>
      <button className="btn btn-primary mb-4" onClick={() => setShowModal(true)}>
        Add New Campaign
      </button>

      <div className="campaign-list">
        {campaigns.map((campaign) => (
          <div key={campaign.id} className="campaign-item mb-3 p-3 border">
            <h5>{campaign.title}</h5>
            <div dangerouslySetInnerHTML={{ __html: campaign.content }}></div>
            <button className="btn btn-danger mt-3" onClick={() => handleDelete(campaign.id)}>
              Delete
            </button>
          </div>
        ))}
      </div>

      {showModal && (
        <div className="modal show" style={{ display: 'block' }}>
          <div className="modal-dialog">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">New Campaign</h5>
                <button type="button" className="close" onClick={() => setShowModal(false)}>
                  <span>&times;</span>
                </button>
              </div>
              <div className="modal-body">
                {error && <div className="alert alert-danger">{error}</div>}
                <div className="form-group">
                  <label>Title</label>
                  <input
                    type="text"
                    name="title"
                    className="form-control"
                    value={newCampaign.title}
                    onChange={handleInputChange}
                  />
                </div>
                <div className="form-group">
                  <label>Content</label>
                  <textarea
                    name="content"
                    className="form-control"
                    value={newCampaign.content}
                    onChange={handleInputChange}
                    maxLength={5000}
                    rows={5}
                  ></textarea>
                </div>
              </div>
              <div className="modal-footer">
                <button type="button" className="btn btn-secondary" onClick={() => setShowModal(false)}>
                  Close
                </button>
                <button type="button" className="btn btn-primary" onClick={handleSubmit}>
                  Save changes
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default CampaignPage;
