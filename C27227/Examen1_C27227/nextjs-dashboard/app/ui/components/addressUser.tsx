import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import axios from 'axios';
import '../Styles/address.css';
import PaymentMethods from './payment_method';
import { decodeToken, checkTokenDate } from '../../hooks/jwtHooks';
import { useRouter } from 'next/navigation';

const AddressForm: React.FC = () => {
  const [street, setStreet] = useState<string>('');
  const [city, setCity] = useState<string>('');
  const [zipCode, setZipCode] = useState<string>('');
  const [specificAddress, setSpecificAddress] = useState<string>('');
  const [country, setCountry] = useState<{ value: string, label: string } | null>(null);
  const [state, setState] = useState<{ value: string, label: string } | null>(null);
  const [showPaymentMethod, setShowPaymentMethod] = useState<boolean>(false);
  const [warning, setWarning] = useState<string>('');
  const [countries, setCountries] = useState<{ value: string, label: string }[]>([]);
  const [states, setStates] = useState<{ value: string, label: string }[]>([]);
  const [isMounted, setIsMounted] = useState<boolean>(false);
  const router = useRouter();
  const URLConection = process.env.NEXT_PUBLIC_API || '';
  const geonamesUsername = 'yuuta';

  useEffect(() => {
    setIsMounted(true);

    const token = sessionStorage.getItem("sessionToken");
    if (token) {
      const decodedToken = decodeToken(token);
      const isTokenAlive = checkTokenDate(decodedToken?.exp);
      if (!isTokenAlive) {
        sessionStorage.removeItem("sessionToken");
        sessionStorage.removeItem("expiracyToken");
        router.push("/admin");
      }
    }
    fetchCountries();
  }, [router]);

  const fetchCountries = async () => {
    try {

      const response = await axios.get(`http://api.geonames.org/countryInfoJSON`, {
        params: {
          username: geonamesUsername
        }
      });

      if (response.data && response.data.geonames) {
        const countryOptions = response.data.geonames.map((country: any) => ({
          value: country.geonameId,
          label: country.countryName
        }));
        setCountries(countryOptions);
      }
    } catch (error) {
      throw new Error('Error fetching countries: ' + error);
    }
  };

  const fetchStates = async (countryCode: string) => {
    try {

      const response = await axios.get(`http://api.geonames.org/childrenJSON`, {
        params: {
          geonameId: countryCode,
          username: geonamesUsername,
          featureCode: 'ADM1'
        }
      });

      if (response.data && response.data.geonames) {
        const stateOptions = response.data.geonames.map((state: any) => ({
          value: state.adminCode1,
          label: state.name
        }));
        setStates(stateOptions);
      } else {
        setStates([]);
      }
    } catch (error) {
      throw new Error('Error fetching states: ' + error);
    }
  };

  useEffect(() => {
    if (country) {
      fetchStates(country.value);
    } else {
      setStates([]);
      setState(null);
    }
  }, [country]);

  const isValidAddressFormat = () => {
    const zipCodePattern = /^[0-9]+$/;
    return (
      street.trim().length >= 5 &&
      city.trim().length >= 2 &&
      zipCodePattern.test(zipCode) &&
      country &&
      state
    );
  };

  const validateAddressWithAPI = async () => {
    try {
      const response = await axios.post(`${URLConection}/api/validate-address`, {
        street,
        specificAddress,
        city,
        state: state?.label,
        zipCode,
        country: country?.label
      });

      if (response.status === 200) {
        return true;
      }
    } catch (error) {
      return false;
    }
  };

  const handleContinueBuy = async (event: React.FormEvent) => {
    event.preventDefault();
    if (!isValidAddressFormat()) {
      setWarning('Por favor, ingrese una dirección válida.');
      setTimeout(() => {
        setWarning('');
      }, 2000);
    } else {
      const isAddressValid = await validateAddressWithAPI();
      if (isAddressValid) {
        setShowPaymentMethod(true);
      } else {
        setWarning('La dirección proporcionada no es válida.');
        setTimeout(() => {
          setWarning('');
        }, 2000);
      }
    }
  };

  const handleZipCodeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value.replace(/\D/g, '');
    setZipCode(value);
  };

  if (!isMounted) {
    return null;
  }

  return showPaymentMethod ? (
    <PaymentMethods
      address={{
        street,
        specificAddress,
        city,
        state: state?.label,
        zipCode,
        country: country?.label
      }}
    />
  ) : (
    <div>
      <form className="address-form" onSubmit={handleContinueBuy}>
        <div>Shipping Address</div>
        <Select
          value={country}
          onChange={setCountry}
          options={countries}
          placeholder="Country / Region"
        />
        <Select
          value={state}
          onChange={setState}
          options={states}
          placeholder="Province / State"
          isDisabled={!states.length}
        />
        <input
          type="text"
          value={city}
          onChange={(e) => setCity(e.target.value)}
          placeholder="City"
        />
        <input
          type="text"
          value={street}
          onChange={(e) => setStreet(e.target.value)}
          placeholder="Street"
        />
        <input
          type="text"
          value={specificAddress}
          onChange={(e) => setSpecificAddress(e.target.value)}
          placeholder="House, apartment, etc. (optional)"
        />
        <input
          type="text"
          value={zipCode}
          onChange={handleZipCodeChange}
          placeholder="Postal code"
        />
        <button type="submit">Submit</button>
        {warning && <div className='alert'>{warning}</div>}
      </form>
    </div>
  );
};

export default AddressForm;
