'use client'
import React from 'react';
import '../styles/Footer.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faInstagram, faGithub, faFacebook, faXTwitter } from '@fortawesome/free-brands-svg-icons';

export default function Footer() {
  return (
    <div className="footer-basic">
      <footer>
        <ul className="example-2">
          <li className="icon-content">
            <a
              href="https://discord.com/"
              aria-label="Facebook"
              data-social="Facebook"
            >
              <div className="filled"></div>
              <FontAwesomeIcon icon={faFacebook} />
            </a>
            <div className="tooltip">Facebook</div>
          </li>
          <li className="icon-content">
            <a
              href="https://store.steampowered.com/"
              aria-label="Github"
              data-social="Github"
            >
              <div className="filled"></div>
              <FontAwesomeIcon icon={faGithub} />
            </a>
            <div className="tooltip">Github</div>
          </li>
          <li className="icon-content">
            <a
              href="https://www.instagram.com/"
              aria-label="Instagram"
              data-social="Instagram"
            >
              <div className="filled"></div>
              <FontAwesomeIcon icon={faInstagram} />
            </a>
            <div className="tooltip">Instagram</div>
          </li>
        </ul>

        <p className="copyright">Abacaxi © 2024</p>
      </footer>
    </div>
  );
};
