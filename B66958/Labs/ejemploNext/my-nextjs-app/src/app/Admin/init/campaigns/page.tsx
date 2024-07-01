'use client';

import WebSocketMessage from "@/app/navbar/WebSocketMessage";
import DataTable, { TableColumn } from "react-data-table-component";
import { FaBan, FaCheckCircle, FaPlus } from "react-icons/fa";
import "../../../css/campannas.css";
import { useEffect, useState } from "react";
import { Alert, Button, Col, Container, Form, Modal, Row } from "react-bootstrap";
import { checkTokenDate } from "@/app/hooks/jwtHooks";
import { useRouter } from "next/navigation";

const Campaigns = () => {
  let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV;
  const router = useRouter();

  const [isErrorShowing, setIsErrorShowing] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [messages, setMessages] = useState<WebSocketMessage[]>([]);
  const [campaign, setCampaign] = useState<string>('');
  const [selectedCampaign, setSelectedCampaign] = useState<string>('');
  const [show, setShow] = useState(false);
  const [showConfirmation, setShowConfirmation] = useState(false);
  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);
  const handleCloseConfirmation = () => setShowConfirmation(false);

  function checkTokenStatus() {
    var token = sessionStorage.getItem("sessionToken");
    var expiracyDate = sessionStorage.getItem("expiracyToken");
    var isTokenAlive = checkTokenDate(Number(expiracyDate));
    if (!isTokenAlive || token == null) router.push("/Admin");
    else return true;
}

  const handleDisableMessage = () => {
    disableCampaign();
  };

  const handleShowConfirmation = (id: string) => {
    setSelectedCampaign(id);
    setShowConfirmation(true);
  }

  const headers: TableColumn<WebSocketMessage>[] = [
    {
      name: "ID",
      selector: (row) => row.Id,
      sortable: true,
    },
    {
      name: "Text",
      cell: (row) => <div dangerouslySetInnerHTML={{ __html: row.Text }}></div>,
      selector: (row) => row.Text,
    },
    {
      name: "Sent At",
      selector: (row) => row.SentAt,
      sortable: true,
    },
    {
      name: "Campaign Status",
      selector: (row) => row.Enabled,
      cell: (row) => (
        <button
          onClick={() => handleShowConfirmation(row.Id)}
          className="btn btn-icon"
          disabled={row.Enabled ? false : true}
        >
          {!row.Enabled ? (
            <FaCheckCircle color="green" />
          ) : (
            <FaBan color="red" />
          )}
        </button>
      ),
    },
  ];

  async function getData() {
    var token = sessionStorage.getItem("sessionToken");
    const res = await fetch(`${environmentUrl}/api/campaigns`,{
      method: "GET",
      headers: {
        authorization: `Bearer ${token}`,
      },
    });
    if (!res.ok) {
      throw new Error("Failed to fetch data");
    }
    return res.json();
  }

  const fetchData = async () => {
    try {
      const result = await getData();
      const formattedData = result.map((message: any) => ({
          Id: message.id,
          Text: message.text,
          SentAt: message.sentAt,
          Enabled: message.enabled

      }));
      setMessages(formattedData);
    } catch (error) {
      setErrorMessage(String(error));
      setIsErrorShowing(true);
    }
  };

  async function disableCampaign() {
    if(checkTokenStatus()){
      var token = sessionStorage.getItem("sessionToken");
      try {
        const res = await fetch(
          `${environmentUrl}/api/campaigns/${selectedCampaign}`,
          {
            method: "DELETE",
            headers: {
              "content-type": "application/json",
              authorization: `Bearer ${token}`,
            },
          }
        );
        if (res.ok) {
          await fetchData();
          setErrorMessage(`Campaña borrada`);
        } else {
          setErrorMessage("Error al borrar la campaña");
        }
      } catch (error) {
        setErrorMessage(String(error));
      }
      setIsErrorShowing(true);
      setShowConfirmation(false);
    }
  }

  async function createCampaign(){
    if(checkTokenStatus()){
      const message = {
        "text" : campaign
      }
      var token = sessionStorage.getItem("sessionToken");
          try {
              const res = await fetch(`${environmentUrl}/api/campaigns`, {
                  method: 'POST',
                  body: JSON.stringify(message),
                  headers: {
                      'content-type': 'application/json',
                      'authorization': `Bearer ${token}`
                  },
              })
              if (res.ok) {
                  await fetchData();
                  setErrorMessage(`Campaña creada`);
              }
              else { setErrorMessage("Error al crear la campaña"); }
          } catch (error) {
              setErrorMessage(String(error));
          }
          setShow(false);
          setIsErrorShowing(true)
    }
  }

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <>
      <Container>
        <Row className="my-4">
          <Col>
            <h1 className="text-center">Listado de Campañas</h1>
          </Col>
        </Row>
        <Row className="mb-4">
          <Col className="d-flex justify-content-end">
            <Button variant="primary" onClick={handleShow}>
              <FaPlus />
            </Button>
          </Col>
        </Row>
        <Row>
          <Col>
            <DataTable
              columns={headers}
              data={messages}
              pagination
              highlightOnHover
              responsive
            />
          </Col>
        </Row>
      </Container>
      <Modal show={show} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Nueva campaña</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group className="mb-3" controlId="name">
              <Form.Label>Mensaje:</Form.Label>
              <Form.Control
                as="textarea"
                rows={3}
                value={campaign}
                maxLength={5000}
                required
                placeholder="Ingrese el mensaje de su campaña... se admite código HTML"
                onChange={(e) => setCampaign(e.target.value)}
              />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Cancelar
          </Button>
          <Button variant="primary" onClick={createCampaign}>
            Crear
          </Button>
        </Modal.Footer>
      </Modal>
      <Modal show={showConfirmation} onHide={handleCloseConfirmation}>
        <Modal.Header closeButton>
          <Modal.Title>Eliminar campaña</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          ¿Está seguro que desea eliminar la campaña publicitaria?
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseConfirmation}>
            Cancelar
          </Button>
          <Button variant="primary" onClick={handleDisableMessage}>
            Confirmar
          </Button>
        </Modal.Footer>
      </Modal>
      {isErrorShowing ? (
        <div
          style={{
            position: "fixed",
            bottom: 20,
            right: 20,
            zIndex: 9999,
          }}
        >
          <Alert
            variant="danger"
            onClose={() => setIsErrorShowing(false)}
            dismissible
          >
            <Alert.Heading>Información</Alert.Heading>
            <p>{errorMessage.toString()}</p>
          </Alert>
        </div>
      ) : (
        ""
      )}
    </>
  );
};

export default Campaigns;
