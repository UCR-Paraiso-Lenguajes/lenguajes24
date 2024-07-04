'use client';
import { checkTokenDate } from "@/app/hooks/jwtHooks";
import { PaymentMethod } from "@/app/types/Cart";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { Alert, Button, Col, Container, Modal, Row } from "react-bootstrap";
import DataTable, { TableColumn } from "react-data-table-component";
import { FaBan, FaCheckCircle, FaPlus } from "react-icons/fa";

const PaymentMethods = () => {

    let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV;
  const router = useRouter();
  const [isErrorShowing, setIsErrorShowing] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[]>([]);
  const [selectedPaymentMethod, setSelectedPaymentMethod] = useState<PaymentMethod>(null);
  const [showConfirmation, setShowConfirmation] = useState(false);
  const handleClose = () => setShowConfirmation(false);
  const handleShow = () => setShowConfirmation(true);

  function checkTokenStatus() {
    var token = sessionStorage.getItem("sessionToken");
    var expiracyDate = sessionStorage.getItem("expiracyToken");
    var isTokenAlive = checkTokenDate(Number(expiracyDate));
    if (!isTokenAlive || token == null) router.push("/Admin");
    else return true;
}

const handleShowConfirmation = (paymentMethod : PaymentMethod) => {
    var updatedPaymentMethod : PaymentMethod = {
        paymentType: paymentMethod.paymentType,
        isEnabled: !paymentMethod.isEnabled
    }
    setSelectedPaymentMethod(updatedPaymentMethod);
    handleShow();
}

const headers: TableColumn<PaymentMethod>[] = [
    {
      name: "Payment Type",
      selector: (row) => row.paymentType,
      sortable: true,
    },
    {
      name: "Status",
      selector: (row) => row.isEnabled,
      cell: (row) => (
        <button
          onClick={() => handleShowConfirmation(row)}
          className="btn btn-icon"
        >
          {!row.isEnabled ? (
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
    const res = await fetch(`${environmentUrl}/api/paymentmethod`,{
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
      setPaymentMethods(result);
    } catch (error) {
      setErrorMessage(String(error));
      setIsErrorShowing(true);
    }
  };

  async function updatePaymentMethod(){
    if(checkTokenStatus()){
        var token = sessionStorage.getItem("sessionToken");
        
        try {
          const res = await fetch(
            `${environmentUrl}/api/paymentmethod`,
            {
              method: "PUT",
                body: JSON.stringify(selectedPaymentMethod),
              headers: {
                "content-type": "application/json",
                authorization: `Bearer ${token}`,
              },
            }
          );
          if (res.ok) {
            await fetchData();
            setErrorMessage(`Método de pago actualizado`);
          } else {
            setErrorMessage("Error al actualizar el método de pago");
          }
        } catch (error) {
          setErrorMessage(String(error));
        }
        setIsErrorShowing(true);
        handleClose();
        await fetchData();
      }
  }

  useEffect(() => {
    fetchData();
  }, []);

    return <>
        <Container>
        <Row className="my-4">
          <Col>
            <h1 className="text-center">Listado de Métodos de Pago</h1>
          </Col>
        </Row>
        <Row>
          <Col>
            <DataTable
              columns={headers}
              data={paymentMethods}
              pagination
              highlightOnHover
              responsive
            />
          </Col>
        </Row>
      </Container>
      <Modal show={showConfirmation} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Desactivar método de pago</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          ¿Está seguro que desea desactivar el método de pago?
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Cancelar
          </Button>
          <Button variant="primary" onClick={updatePaymentMethod}>
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
}

export default PaymentMethods;