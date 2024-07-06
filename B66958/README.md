# Andromeda Store

Andromeda Store aims to be an E-Commerce for everyone interested in shopping online electronic devices, whether it is finding a new computer, a new keyboard, or even a colorful mousepad for your new gaming set up.

## Composed of

- SQL Server Database
- C# API
- NextJs Web Client

---

## Architechture

The project was worked under the 3 tier architechture for the backend, splitting the solution in the following tiers:

- API tier, in charge of handling the requests that the system may receive from the web client. Some of these requests may require an authentification in order to be autorized to be used, but some of them were public endpoints, although the security aspect will be found later in this document.

- Business tier, in charge of validating the data that not only is received, but also returned, so that there is guarantee in the handling of information

- Data tier, in charge of the persistence of data, accessing the repository, writing and reading data.

---

# Diagrams

In this section, we will find a diagram for each of the tier of the backend solution, plus some packages that are considered important to mention.

---

## Api Diagram

This tier holds the multiple controllers and their endpoints for granting the communication possibility through HTTP requests.

![Api Diagram](https://www.plantuml.com/plantuml/dpng/bLJ1Rjim3BtxAtICaUsyIn3KSveXG8OLgRlBi0DZ4QuGMV94Ue1HzDzdHLPIny7Qw29QV8zyJo9yHmoEsredIGFb0IeKMzX_4Ol53ai6KsbSBiLB8aHCPuswZl3hXCgjOMUrHaSzScsrH1TXmoer48aS79_18u1VymBeiDXa9IjhTaYjvgN87GAZPsRKcNBc8r62u_GD-tNIwX4qdbjzgDCtPC-Yq6S5Z8MgKKWVsBKoChseylclLLBy1AqynXCkMJ7tZbQsxdzYZ_cZF1_CqilyvjwqDJhOQrmyE2lRChWPOig1iR9E8SssYdYXJ9ylJuUV1B5JfX9_MdJTM6eGcBNyVEDbRcsbJBZ9NsjdwwzMTh_54w7pnKJR1vFhV0momeiNze0cwCqrgBfNx3UwfY-7kW5LcHirXaQHF14R4w8SWlGZHLUeyRBeO4p9D0cSjoyMeSlZcyKl2inhRX-Wgz7mT-HdAs-q_DJuSHnLcFWUPNVOJmMnS65Faz6qZQ1qD91fFJvzrqFiyPBCbU6OAhe6JyJvtbejDdHlV5kPn3B3XDzu2PcK0t7gFPolYX-Y2M4wbd_UNtiLTsYa_v_-0m00)

---

## Core Diagram

This tier is in charge of validations and most of the business logic for the system flow of work.

![Core](https://www.plantuml.com/plantuml/dpng/fLLHRzem47xth_1Zgj2zxc4eAeDsciPe1T7UZ_WMh3fEPZlT4Ef_dnt7n26qBnMIyTtttVc-yndkj05bsagG1efdA96kOV-PpkTRB0p8Kc2MqTCPNDOAn-20VMqrbwYrnHzgmL0vi11WFKjxnXa-aLjdhy00BVeDIR_VoxP21Nk1yoTLixOm6UqtcgHnXkI6f3dev_aE15fIgue3Q5peeomIH-pItf3Rbvep-Wi4Ps3G0M-ZJt2iK9gVQ0urIqQMfariNaQw8XAtK6qNYsgZk2pfDvIeB1xAsxJL7fMj_RLlYei9acnFdDdrH3EIEcCYDsgHbvxPGupRQQ_TXcFEAwJCRUe7Bjcrs1zujCMtLZDNSr_jZ38iU0KYyqdpse1uD_Ea3uvooGlO83fNY9kQuOca_UM6IoPfmEWQVyVcbfU7o4v2OEE_X-Gzi4EmBNTqszOGHLLTcoyaxMfUCDPpfyc6Y8XrzacxpEWqwEhu-aAIVY3-jAYEeMiUkimRCsTqpRKPabLWYWE6YlIWUWbL0xoKqIm5btzZaK7I04scAm2PqnqCV_ixNvR1VsRAtc9Ll-3L07vr_0BLJrgm-a4RP8tVO2HkjQTiJz8f7ikXzz8TDYBv4yPnd_9ZWxI9VSuJigou_c2QMFauffE_46BixJemdkT8Ut1r5ziCUyAbGIL1K8O2Ip18kp5xb1ly--Qedz_WPVU6gPk-b5N-639CVaJVORvsdpkKpFtP_0S0)

---

## Database and Exceptions Diagrams

This tier is the one that writes and reads data for the correct functionality of our system.

![Database and Exceptions Diagrams](https://www.plantuml.com/plantuml/dpng/ZLHDZzCm4BtdL_ZOQdUvSY14IviMhIWVeWNE4sVKjUeumJCfL5N_E_OwJfFT852askDvxyt7sBqd1ijjfPC6v07sA3PGl1Pf-XKbWzbhp39nxfqvC3ZdkjObsYmx9r83aLWw3Uyw9pSxe8FOmX7VqSd8sHOq2d8_y-JIWVF2mOYjCdktNzR6e6HLc-tJKN9t9WPMyf8Nui6ulP7u9hc9fyTQbMBHAbtcnQnJePzwY_Q8zhdOFBdxBrIl7XRn56Hf4HZpnMp-BFfQQVq5JXKQ_eZyM9Sq3R5rsKhspgOjj9FTAE9qpCp4U-I_YmL640hGgsXezRUc1CQHGkZyM5GqG_Cw33-VCBbn0QxpQDbx-r3fmygq5LeeDAOVyFGTTEi2Avk61j-A4gMgG6UPJpMG5wSVY8UGaOk3ErMXy1dxuobXpnhnFMk2lwlNoWmAMKBLWDgRuJKCHKHF48cMaDqc9X4TcSXH8-E85k-GAgToZj2GTxGeCFzDs03nxj4Ylc1UagwwRko3ugwt8bJPcO7P6Q89wstaBrfI1ebUVJQVy5T4rKFZhJ2jrYwq-_fdl_ejiV4FXYOU_wfg-DGZpl6oZuzZ0oiaSehNkSMKNWgCFTCQuRj7K_g_i3y0)

---

## Models Diagrams Part 1

![Models Diagrams Part 1](https://www.plantuml.com/plantuml/dpng/XLJDZjiy3BphANXqu2_VjPVKtV_2mEuss6qVGB7Oh51PCYHv2yFDkvUo8aTk3JIvc3FaYEBGldAUMz-tYdMy-S6F28_yy05sknTiFDT7XLK5uqm-6O7A4VrWb41RLIDh57SERabbP42_yb4wlmlntXhHDxw2yqCjNCnmtafzX6iXBBepjkT3Yze_eNypmlt_TUWG5jYYzDRexzAst4kZd_lsW9QT675q2hNYyMZiCB9IQW-rOAdgcRV8jeal0Yl5Vt1cDR4RLexEasnpIcbmqqibrfDFwVOnLw8RsJONY218JPXkwF1fE4cwWcREOULANJtNNS1YmxRlHehOrmfEO_t75UjxtRTe-K5Xrijdz0krCb4PCzmCTPf5k3u_a3AdeI_jez4Z7G71HLRcNaRaO8o2sjthq0BvC_KT8peuXdVIfNWfNgojIZVBvnukC-51Qh5QxOSkS_DLwWup8oDmCR5S4fbBKrpaU-7USg4Gvpevdic4C5Fvvj1UzvwKojTiaLEmfvoVneePU36As5HLPDxFYz2TQnBm2ImLmKVGlL8qm29sje79YjhjBJhKlaYYqDD3Ta-BWcZ9rPSkl99kN2vPAglW3jzHcIuumCg_oehD6hYk92vAeP4_hDxzscwdiQuHqJV6iZVD49fsd9teVuLQXC_YRm00)

---

## Models Diagrams Part 2

![Models Diagrams Part 2](https://www.plantuml.com/plantuml/dpng/nLHVJzim47_tf_1ZKTBdlLGHq0vK2HYmyW4S-rQyENRcFzEggDzzJh2Jk6a724rJfSPtvxlVx_xupeqbshfAe9hG7sGB-8QKd_1YyGZK4haLK1IuwOstYe4mtdob10DT50sYWXY3xxLYZje6PSPgBhVuZbJG2-lAEpzfWJ86b5T4-FkS3lOL6AfvRRcIABjsdE4d_uUo9R6mLNg7um5bglpkcU6bK19cPsYVugysNpvEuIXk0C0R5KuZy2NHDXOWy5-p2H2N5Zyu8YstkqFUfa7pnbXYETttEiobLqdlEx_XnYw2li1rk83cAqvRnaJl5ZvyZfDxHH_fSjSdCeywpp-RGgvxo5_Aft75XHWSWlA2iLY3y8sCVCZrP-agqAGKC12-1ZkbCkl8Skdjz4N5mRmfmeC3xnuQzxCLtWLy8PcFS890ZjyPjvk3qCslH41pmYNUJysZqIdmI33zOFfgQp1c6AUBIZbf_KIINGNIte9zLipWHEozxvscpyJ0dQjAqBrwgUGthYlI3azgwd9O4Uide3tDJfAb8x9vy2KlNFD8bNHKymEczLYChdL2D6hfXETPu1KkCCyqnmTu-7XCFCfb-fZ6pw55m9UE2tOw__f_vDvJ71XUAVq8jTBM93L9QPn0UgKW-pZMLkbshjCj59k_bgqzBBWD-NqFceAqP0lH8oc11qXab2LzYO-qmE748IAC6jH2BPtMFaoAE6pHLObPENBfGepsMoVFZY_MV99A_ePCykDvlBxS_icA-k32_VksJQlQhS3sTmwIkKgWFm00)

---

## Tests Diagrams

![Tests Diagrams](https://www.plantuml.com/plantuml/dpng/fPPFRzf04CNl-HG-jgfwh7f1KHmW3MhYe02Dr4iqx0xsYlMkDJCcq2ZVlJRnHdM3vLeL4XBhztkpy_QFkM01aZAtPmMeBQGOtS3wQpGQtQCISAd5y_Fe-UtXQldkaRB078r1CFLqM28BFvzzsdcZemNAglZmiVaL90z6ifd-YUGVbndv9uufBNDqCjqhBCHuTn99V08v3a9guAgqzcxpa1d1HTK5yZyxN0F_06jqKFBZn2CdNleCvkHrgOGl3y6gXuoThee5hF9vQLATbwGoO6nINOWdZBZ-xaYOgX8WbLdYWrrVlsqgrfgG1q9pEDIgMvJCwm7ey7oNmDje0HO7jDbG8Skw_74BEb1YTdXgyfSb6uVCqts1whX2V72gqsfXN86l1h7JYHzxjp6KGqqcPRv6wlDzENsCFjzwjHtDxewTy6XCgD690TkCL0TG_JrM8m6yzYnJjpFaNTtZUPIr1sfLUoU5Y4k5p9lIshYKhFPNnsxkKKfoVFVzJNdbQMsqHdTIk78WcIVp2tMhoxm0axfmMRHhZmb1C6XUZ_uIzz9as5hP3g2Mtm2bICiCt9VjE0C29KZSQNAFkT-rJAPv8OUP7bWsRE8kwXjAR6sWk2N_KnsoQctFK_T0-mgO68QrnHRLKICWK_TVHEBTT6zO0dmoi32tOLpOOYWgCovjJR4_kN0pz8SMQrtP9_ZK8FLQ-bB2--PzCztCB8SKLsI7SXDaHUOe7OhEoIWChzCXOBXi1hDZJuHA9Y3GlGh7pmKwNV-v-0q0)

---

## Services Diagrams

![Services Diagrams](https://www.plantuml.com/plantuml/dpng/RL4zQyCm4DtrAswC2Pwx694s9H18boQqepbBXn6MfQ2x98F9Vw_hA4vRYWRnFlJU0wrOCCcfz-g8fiEMO8VD4rJLEnd1q7hI6ePPt5CwEqCy6ZRHMqfQ3yfuP8PdQnCnVw1t5YMcGIqNB2ZE5D34w24hb8sRc7gKqP9SQ05lPA6kEUwJcdqq7Kck75JvYw84DK-tAdUEfThEyZg6CAvtCMYeB_o0hCerzaTqRNX5GQZD3wIMVollsqx95sfvZix2Iufe3RAy4JEsiwM_mKAL1-GEzXJiDZXnwE-5lFXl85muHqzlScfrq8l38_CwdTMeV__J5m00)

---

## Package Diagram

![Package Diagram](https://www.plantuml.com/plantuml/dpng/VLBHRi8m37pdA_YG2i60HyQqiktP9ASs8aqgsvs69lvzwzQKjgfGN-ppNNq-PIrAh4tb5pMP2nLORYCZorvXb4BXiLejl_lPKoCkG2JB4l8tDfv4bbkgQd95IAGUPfr0BtIj4FG0BQELwP2ZRSnKafF7F347iIbF37HuMpv7YyMjzxqZfOUUMyB0xw3TdKVhqYXvQjl1elsNGQqkXlc8zbMjrvxJWUciEtx_to5Q-CTn3sptfQA8V1st3lFnfsDSgD4hf1nVFUva4idfE30nLhGPfL8Zf_fDm9j6owHZM0Hrv8__wGqYoC6Vpc0-X8sr395tyiwIHkxW3vppQ2xGJhou_Nvh1Di-z1y0)

---

## Sequence Diagram

- **Sale Case**

![Sequence Diagram](https://www.plantuml.com/plantuml/dpng/bLF1ZXCn3BtFL_W7-84Uq5PLGI21Lgt45Rc9LMNbYMSTJyNoHnmuSUFQ7oFPAQEPKKhfPTGqVi_FyNiFoL2jRxb1PwAmuK3HgEbGBRZGOJJuJD_iO_ewNCrkrbsOdJUYr7WqF60Ys3_pdlH4sZGNsZTlvsIhZ8YfPqFeL7oV9QIcA0adD5eqRcR7J9Mxh-0z6UpekQTau0aUApH5PPC_6L84lc8kTPEoGNnJlhbmrBw2lN2Upn0u94ClfOs9bmGigQPxElZuSywvVIBN6qBMhI_N2AVOGRYIYKQ43zlOjwHuO5eCxQcceg271xkJOtYJZ5jkxawUpB3O7sJ6YojMq9RrwSG4G16Aev0P1wAg9QxxQdrKEau9aS6XQZ2v2V91oRbm_XNBGFcc_7iJrPv_c0pkwF3uZouxGWxV4HZ1ITif_gVZuUy67sNm-cQ2hXjz6q_ddmXoU28JsA1QrUzZ_HVauD45YPGgIPZJxP7fBjElkKL7SJ9vnUBlGbmyppLJ--8PULdwV9BoI8hGHuZdtotfwsgKN3Z8B12xYv69AuNBC8nThoRZ3iAC7iUeH6IQa4z3GkgS8yV2FWyKVT_o7m00)

---

- **Searching for a product by category/ies and specifying a query**

![Search Diagram](https://www.plantuml.com/plantuml/dpng/fP9DZjGm48NtFaLT0B7l1Heq6X1884HBR55DN7GREQwCNHxHHsB1AUPYb3jM55iPK60JnJ_rlLUkTvC4e-JH6no48znwHq78JBhh1ZTX4FX8F-Hz-ZeVDIUl9zUiRpcIiIXuZudW-E2F51yf6bEnBrwri8DMX9Iz84oHRLOBoHGdxX65Ec7JB1MbwWTuImAVwI5J4h04dpOmnQNHZvOKWzUQUczrRJPlobSlBjuFS6Il_Gc2TqdGSf4Hjfp0SzhmtJI-_BRCk-yqP4MgxtXv1jZKpiOZ2SU0yEukv94YtdlgchQqvS9IaiYNzLO1Bjm1XpDXAmyKOAIHeyFbjZxYJXFpesfTF_gT72Lq0G0brbWTOCobGspTbqYYzzY06iQkOC8tvrKZM8Q98mnAEt5y-eMW4tGd9zUZ7EQX_bUCLw8JHWIFBpscbMI2Iokp5VCskF-ABQmkBb_GE_jCEVbAQ4lUbBVh4sRfchoddtyFsC1XLQWASCOJQa9HXlD6uchYT-PlXumY5vMDTAx3-Q66KvlluJeVYbpvyr1Iha6p_AVWLkb--uQ2pQF_3G00)

---

- **WebSocket interactions**

![WebScoket Diagram](https://www.plantuml.com/plantuml/dpng/lPN1ZX9148RlynJr0hu01xCuwm4FgrcIzMYAxf9KdEaUguh9hk_amNZpoelP3QETGM0x7lOmX0xzVtyL_q-uKKEnNTyrw2mAj1rJC9fEIzzpQ8PqWnqF60pkwD7UwQVZjTadom-hsRcDGer7mmqgmVfhjoOPIPfcidZrUWw31Jomii60suC_ZsXqOZXN94HoNIIhG8-y_ndW8stMqNqXAu0yLthItJnHSdc_CGeaiF_L6VUeO48A3li1zpzGY_AmGNv9HcK1EFZfhZE48Px1u2cdx5uWPPnxiYW1BtCydI5byRH_DQ8ibLHdllwtOTn_hwCamBzPJC4ku9PqIE-tTQnvmisXSt1cl5LWnn4yWPBcj2-Lv2AX5KBvc_IbCWcXKPilyJOizIcuAsLwm8vzWZx1GAAiniMWeache2I6HLFRcdQw3tmaU7o-9qNqPrQFbTMuYyQVsQ40WaKV5Rhqk6FW2YDZ-UazLmpQ9apOqOXbwtcrhfJWJHH9ylPwXhVKqVzasAFa7LtiSy_qAF4rGIwTxHBc6qff-ILEVRWuKUlZdSjUEjzxwkD8fSspvKyplg7Wyn_FRm00)

---

## Payment Methods UML Diagram

![Payment Methods Diagram](https://www.plantuml.com/plantuml/dpng/jLN1Rjim3BtdAmIVaYqUi6iG6CkQTZ2m0a6MVe1i4vbNHV8a6bXGzDz7nPuJAchWG-T3w_0zqe-aI7rnA2ss1pLfPFaizo2IrION6sj-GOc9M2wtz5_glO8i4oyJGS_0l3CQhL4AhFFC1wCgi0FDFwMIpeaVQ2oSoHq-F1-_0PuOqvbOTDm1Vvq4eJRoU02DZu0_JVLco9JHtDVMrHh8kucQkPgdff88BC3A7NKvjV2x1OUaaO7RpkorxwHxNkQh4ckZj-1QXTbLfYkbkBXJrlczRWzWPQ5WoU7ieXRTNwh9HGSiZ6pMewb0ZUXJs9tljSCbVQ9gIyn4_-9oVI9pQgsHQl1d0xO4ZVJLM2IUNT0Ixd1PbJk9i3Vsc8d-hOQu28SIwxBF9TVqKvUSalQKTI6gWY4l7VBQUK-RFbbg4Al2d6T9C6K-3r8Pd9YLVCj1Fr6YASvkYPgD70uNDXBeIs9tR82RlUMVvpLQ6AD4xkwrLrXnh3DEPzowhXqpixQ44XzgNKtHArW4q2UlPNPbZYJVjphvd8Wq5Sc79E92T3uD_UpVdeVu5WbtpN2CWih4TWWx8hkRUVjb4gdikuHz_wN4gxkMA6zMTbs4ftEOIJQpJ6L4Ocp2ngrpNhEhUh5IyUtx7s_AwCr2f73BZftNcFhmCaxJh9-WXKZELt4oOLe2X_Xa-0XF3Xpvnd7lkYv6qJXqTY7tE1mv87FnPlSflxy0)

---

## Payment Methods Activity Diagram

![Payment Methods Activity Diagram](https://www.plantuml.com/plantuml/dpng/fLR1Rjim3BtdA_W3m-u1CjJ8jc83CXGDrkl0I6pAGPOyYYws_KsFE-oqswxviKLk0jLsQwNT8G4ayJtIV4-qpwAYQ5kx6HeD0fzZYyBXiAfipPwZ2jeWimP5sN23Nk4JVTUFySlICNdjdLGDzzRB83IpgBZ1IB3-vjOajoIpsMAnpqO25wsO6umKuKA2ROr6M2ncXqBWrPj-hZbKMw4jGdCV72AW0uCYhA6CiYna3E_-UB04v3Njbc7rxfS66nAcmUrKdaj2npyH7889TIEOMictg3JeMM-LYAg6vt0HegOiom4sTNN_It6feNDuvsztln72vYjfW2MAvk2kvUalXr-XOuk6WwTuFCfYxCz9-9eDGcGWDtnUeAYxExZ13JjMj27doeAYKrWDS9RY0n00vFXSsoLf49-RF0PP6kTB1nt1eNUPXqI2WAN1KnI84rNQIrnZNV-FF75sMoFH3qQQmwgbgF92_e6ro4KQIL7MuOXuPlFpxj2LQtJtBNsS8Ay6B9MhqP1_S8cUAlaz--5rAvQSqUVaIH1Q3txtjoRfcYLaU1CctEN6qI_rKfdgTEUKsSO-yRORmtdCQn1OeSTjciRueqwvLgGtmNP3kVS-UEJ1FdWsZ10ffWbR3B_Yo8hGxEwsxF61wUFHjKzCoNFIohb3WOVw9cRbEZWsh3YcxoisDdG1Ed1OURPb-ACMxHck0CrbJvYi0DyZnntkgPRjDU8fobjrQLw_fNYVS3ZIdlLcJ6njkkiJD0FXfp05uIUWG1us62cjBMg89pfX6VmrIus0H-RkCwHeX0Aou8C2-dGRb6a6AfwHj-cxyXy0)

---

## Multiple Existences Of Products In Cart Diagram

![Multiple Existences Of Products In Cart Diagram](https://www.plantuml.com/plantuml/dpng/ZLLTJzim57tlhx3wb0gw7p1L4Q4RADAOEc1xD_PTyN2SoX-JAiH_txsD5spJ4lBGf3x7r-SSvpeNpdFhGwTdEowU-1ROlDcqR6FxFo3ydArMjtZdPgkXhjdpZE4rCjUzyRRN6gmZvbMl9TYHHfVGt3csnYLUkGbCrzdQ0lT0dCRjZQW4Fh7FXqaBlDzpzxHg6-5LRsx11UthSVxBRFZDPTtq4lIqebB7D-NyYW0qBuFmDOiFhNGvqtchp9OrKbfm1RRX-mwClm7_s4ltwNw_0vQD7Is5wVnMjkDaydle7i0c7WljYSJImdLGahN54XA4whX6Dqf03YdZsO_0ZLT-Vth1EwvXEgpapuVYoiKrNH-CVo_A3wIu2LOySWT5WjD1ZzUpymY9by7pPL1QLhje2j_5zgi97LZ-e24pVyxuO0uvmq8uSd240z7YETkb1X38_I6UoKzAYKnq1bA7aDQ9bhWCJXbmqqtndrZizvAmBznp9k93dcuQJDa-POD4bwURdVpK-VA_k5OodXBPdEEqBDdg_TnFb20HbSEV6711kmO35cdvArSjnesPs12AS78pY5GstXt_6wDBnSNeZYuQ2-Qkax9dW7l7wA1w7KgFhHmP6gIiTgMCh3mIYtZZ2RXuCnpreSBYux1S3aS9pfWFNmOtFt0YCZOT4MWULmOPsRkRra2sm34umYbD8Fjd9CRiYKJv4eJ14tO1HjBdyXy0)

---

## Carroussel Activity Diagram

![Carroussel Products Diagram](https://www.plantuml.com/plantuml/dpng/hLNHRjim37ptL_07yWC1ChJ8C61x68e5xUl0Mcn3m9OqYNBMlvyLLORL28Ozz251XF3ko3lAllE2JaBVDTYASV3e0pesZKKdtB95BV2J_ieF__lOCMaf_hctN9oFnb6ZKF0PFS7fJtSYDv1hciC1dfWkh5y1jOA7uDep-dXyS4Q5LZmS3atphWwxBwNe_ee6-ruRU-K1XJwqLfn6ajZV7dwHEAQ1CXoSkSmeuWpZ9vR7zlVmZQGzrulJWF4BTZFwu7HPhwX09FxReRwojns-LR6vjG8IIN94UpYTpQMElN4c1d2lL66u63YYasNZ8shIU76cXnQT2PwwJI4KD68YYIn4CFROalVNCa2bMDZeDIa4gr189Q0NbB0wZAFHB-pwtEKako6Gnwjw4bSfsyL8qfnqZUN2SixtWfOpoR1e92hbebCMNsbR8gVmtBDaWaeOjLr8OWDsh725EJDlygxZ3GUCvz39WbEhdB3uraVH_tOY4_GaPwDM6VAKXg0FsCskPFmdkJAjSbgdZNPyromSYpCSRBgEAoviXbJlQYwUN4lNMwiy_bGtlJd6S72OTQ98aBjP9rDbC_s2NLRAHkHLhQ3gp_8irvRuRG_rEicsftiTUxCATwHLV8V_0m00)

---

# Security

Andromeda's Store security is very simple, it was never the goal to have a very robust authentication and authorization process. This is very evident by the fact that the system doesn't have an users module. There are no new accounts and no password recoveries. Therefore, security was based on roles, whether you had one or not. So for those data exchanges that should be done by authorized people, a token was created from our backend when a sucessfull login was made. So let's dig into it.

---

First, since there are now account orientation in the system, our verified accounts are stored in the **appsettings.json** file of the **API** project. So our users had the following structure:

```json
"Credentials": {
    "Development": [
      {
        "Name": "Diego",
        "Password": "secret",
        "Role": "Admin"
      },
      {
        "Name": "Eisenheim",
        "Password": "secret",
        "Role": "Operator"
      }
    ]
  }
```

---

With our improvised database of accounts, now he had to set up the security. We configured **JWT** as our token provider. This is because it facilitates a wide configuration schema for our token, and due to it being one of the most supported and efficient libraries in the current market. So it was set up as this:

```csharp
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        JwtBearerDefaults.AuthenticationScheme,
        options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://localhost:5001",
                ValidAudience = "https://localhost:5001",
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        "TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"
                    )
                )
            };
        }
    );
```

As seen in the code, we decide to validate the issuer of the token, same as the audience, and our main concern: **lifetime**. For both issuer and audience there is a static expected value, since it wasn't our main area of work. Little to no attention was given to the symmetric security key, which was a default value given by the online examples.

---

But now, what do we do with this configuration once a login is attempted? Let's take a look to the dedicated endpoint for such action:

```csharp
[HttpPost("login")]
[AllowAnonymous]
public async Task<IActionResult> LoginAsync([FromBody] UserAuth user)
{
    UserAuth.IsPresent(user);
    if (hostEnvironment.IsDevelopment())
    {
        var validUser = credentials
            .Development.ToList()
            .FirstOrDefault(cred =>
                cred.Name == user.Name && cred.Password == user.Password
            );

        if (validUser == null)
            return Unauthorized();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, validUser.Role)
        };

        var tokenString = CreateToken(claims);

        return Ok(new { token = tokenString });
    }
    return Unauthorized();
}
```

---

The idea is very simple... after validating the data provided in the login, we check on our **appsettings.json** and try to find the user, if our user is found, then we create a list of claims for the token to be created. And such creation looks like this:

```csharp
private String CreateToken(List<Claim> claims)
{
    bool listIsNullOrEmpty = claims == null || claims.Count() < 0;
    if (!listIsNullOrEmpty)
    {
        var secretKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                "TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"
            )
        );
        var signinCredentials = new SigningCredentials(
            secretKey,
            SecurityAlgorithms.HmacSha256
        );
        var tokeOptions = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: signinCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
    }
    else
        throw new ArgumentNullException(
            "The list of claims should not be null and must contain at least one claim"
        );
}
```

Using our default key, the credentials for the token are created, such as the issuer and audience expected by the JWT, and our main concern, the lifetime, which is 5 minutes based upon the UTC time, to avoid server time issues.

---

Finally, if an endpoint needed authorization to be accessed, a role was expected to come in the token, that way we validate two things, who is trying to access the endpoint and its token lifetime.

The validation is as simple as this syntaxis:

```csharp
[ApiController]
[Route("api/[controller]"), Authorize(Roles = "Admin")]
```

---

This meant that our web client had to store that token somewhere, so that when a request needed to attach it, it will be easily retrieved. For this purpose it was decided to use **Session storage**, in there we stored the token and its expiracy date.

```typescript
var tokenString = await response.json();
var token = tokenString.token;
sessionStorage.removeItem("sessionToken");
sessionStorage.removeItem("expiracyToken");

sessionStorage.setItem("sessionToken", token);
var decodedToken = decodeToken(token);
var expiracyDate = decodedToken?.exp;
sessionStorage.setItem("expiracyToken", String(expiracyDate));
```

---

When needed, the token had to be part of the request made to an endpoint, like this:

```typescript
async function getData() {
  var token = sessionStorage.getItem("sessionToken");
  const res = await fetch(`${environmentUrl}/api/campaigns`, {
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
```

---

At the same time, the web client would check the date of the token, so that if it was expired, the authenticated user had to log in again to receive a new token

```typescript
function checkTokenStatus() {
  var token = sessionStorage.getItem("sessionToken");
  var expiracyDate = sessionStorage.getItem("expiracyToken");
  var isTokenAlive = checkTokenDate(Number(expiracyDate));
  if (!isTokenAlive || token == null) router.push("/Admin");
  else return true;
}
```

---

# Cache of products

Given the fact that the store may receive massive interactions and clients accessing it, a decision was made to reduce the amount of times the database was accessed. Although the store is an static instance of a class, it was also decided to have a static cache of products, in this case represented by:

```csharp
public List<Product> ProductsInStore { get; private set; }
```

This list would hold the entire catalog of products available for purchase, so that after the first query to the database, every next time an user would search access the store, the catalog that was shown, was the one in the system's cache.

---

So our flow of returning the product catalog to the web client is as it follows:

- First, the request is made by the client.

```csharp
[HttpGet]
[AllowAnonymous]
public Store GetStore()
{
    return Store.Instance;
}
```

---

- Then, our store instance is created by retrieving the data from the repository.

```csharp
public List<Product> ProductsInStore { get; private set; }
public int TaxPercentage { get; private set; }
public List<PaymentMethods> paymentMethods { get; private set; }
public IEnumerable<Category> CategoriesInStore { get; private set; }
public static int CurrentTaxPercentage = 13;
private static Db db;

private Store(
    List<Product> products,
    int TaxPercentage,
    List<PaymentMethods> paymentMethods,
    IEnumerable<Category> categories
)
{
    this.ProductsInStore = products;
    this.TaxPercentage = TaxPercentage;
    this.paymentMethods = paymentMethods;
    this.CategoriesInStore = categories;
}

static Store()
{
    db = Db.Instance;
    List<Product> products = Db.GetProducts();
    List<PaymentMethods> paymentMethods = db.GetPaymentMethods();
    IEnumerable<Category> categories = Categories.Instance.GetCategories();

    Store.Instance = new Store(
        products,
        Store.CurrentTaxPercentage,
        paymentMethods,
        categories
    );
}
```

This way, next time the instance is accessed, the repository won't be accessed.

---

# Product's search

To give the possibility to the users of the store to search products in the catalog by name, description or even filtering by category, we had to take advantage of our product's cache, however, that implied that the search time for a matching product for the user's query couldn't be large. This is where our second cache of products come in to play.

---

The first time the system starts, a **List** is in charge of storing all the products available, and althought the list is filled by reading the data from the repository, in reality, even before writing the products, our second cache is already available. Let's review the process that allows that:

- Our method FillProducts is called when initializing the system.

```csharp
public static void FillProducts()
{
    List<Product> products = Products.Instance.GetProducts().ToList();
    string insertQuery =
        @"USE andromeda_store;
        INSERT INTO dbo.products (id, name, description, image_Url, price, category)
        VALUES (@id, @name, @description, @imageUrl, @price, @category)";

Many more code to write the data...
}
```

---

- By accessing the **Products** instance, the class's own filling products method is activated

```csharp
public static Products Instance
{
    get
    {
        if (instance == null)
        {
            instance = new Products();
            instance.FillProducts();
        }
        return instance;
    }
}
```

---

- Out of an array with 40 products, an iteration is done and one by one is added to the cache

```csharp
Product product;

for (int i = 1; i <= 40; i++)
{
    var productData = productsData[(i - 1) % productsData.Length];
    int randomIndex = rand.Next(1, categories.Count + 1);

    product = new Product
    {
        Name = $"Product {i}",
        ImageUrl = productData.imageUrl,
        Price = Convert.ToDecimal(productData.price),
        Description = productData.description,
        Uuid = Guid.NewGuid(),
        Category = Categories.Instance.GetCategoryById(randomIndex)
    };

    AddProduct(product);
}
```

---

- The way they are added is using a dictionary, this way, for each category in our catalog, there is a list with the products belonging to that category, and that category only, _since there are no multiple categories by product_

```csharp
private Dictionary<int, List<Product>> ProductsByCategory;

private void AddProduct(Product product)
{
    products.Add(product);
    if (ProductsByCategory.ContainsKey(product.Category.Id))
    {
        ProductsByCategory[product.Category.Id].Add(product);
    }
    else
    {
        List<Product> productsForCategory = new List<Product>();
        productsForCategory.Add(product);
        ProductsByCategory.Add(product.Category.Id, productsForCategory);
    }
}
```

---

- The search made by an user, can be one of three ways.

1. Filtering the catalog by one or various categories

```csharp
Store.Instance.ProductsByCategory(categories);
```

2. Filtering by an specific query

```csharp
Store.Instance.ProductsByQuery(query);
```

3. Filtering by both, categories and a query

```csharp
Store.Instance.ProductsByCategoryAndQuery(categories, query);
```

---

- The category case is simple, we retrieve the list of products assigned to each category in cache:

```csharp
public IEnumerable<Product> GetProductsByCategory(List<int> categories)
{
    if (categories == null || categories.Count() == 0)
        throw new ArgumentException(
            "The categories list should not be null and must contain at least one category"
        );

    List<Product> productsFound;
    List<Product> productsMatching = new List<Product>();

    foreach (int category in categories)
    {
        if (ProductsByCategory.TryGetValue(category, out productsFound))
        {
            productsMatching.AddRange(productsFound);
        }
    }

    return productsMatching;
}
```

---

- When a search is made by a query, we can't make a search for the matching products in lineal time, therefore we used a Binary Search Tree.

```csharp

public class TreeNode
{
    public Product Product { get; private set; }
    public TreeNode Left { get; set; }
    public TreeNode Right { get; set; }

    public TreeNode(Product product)
    {
        if (product == null)
            throw new ArgumentException("The product can't be null");
        Product = product;
    }
}

public class ProductSearchTree
{
    private TreeNode root;

    public void AddProduct(Product product)
    {
        if (product == null)
            throw new ArgumentException("The product can't be null");
        root = AddProduct(root, product);
    }

    private TreeNode AddProduct(TreeNode node, Product product)
    {
        if (node == null)
        {
            return new TreeNode(product);
        }

        int compareResult = string.Compare(product.Name, node.Product.Name);

        if (compareResult < 0)
        {
            node.Left = AddProduct(node.Left, product);
        }
        else if (compareResult > 0)
        {
            node.Right = AddProduct(node.Right, product);
        }

        return node;
    }
}
```

Everytime a product is added to the tree, it is sorted by its name.

---

- When querying, we take the tree and call its own search function

```csharp
public IEnumerable<Product> GetProductsByQuery(string query)
{
    ProductSearchTree tree = new ProductSearchTree();

    List<Product> matchedProducts = tree.Search(query).ToList();

    if (matchedProducts.Count > 0)
        return matchedProducts;
    else
        throw new EmptyException("There are no products matching the queried query");
}
```

- The binary tree searches by deciding which route is the appropiate to find the matching products, based on its first letter

```csharp
public IEnumerable<Product> Search(string query)
{
    if(string.IsNullOrWhiteSpace(query)) throw new ArgumentException("Query can't be null");
    List<Product> matchedProducts = new List<Product>();
    Search(root, query, matchedProducts);
    return matchedProducts;
}

private void Search(TreeNode node, string query, List<Product> matchedProducts)
{
    if (node == null)
    {
        return;
    }

    bool productMatchesNameOrDescriptionWithQuery = node.Product.Name.ToLower().Contains(query.ToLower())
        || node.Product.Description.ToLower().Contains(query.ToLower());

    if (productMatchesNameOrDescriptionWithQuery)
    {
        matchedProducts.Add(node.Product);
    }

    Search(node.Left, query, matchedProducts);
    Search(node.Right, query, matchedProducts);
}
```

---

- When mixing categories and a query, we faced the most challenging cache query. So we will use the binary tree search, but **only** in the categories given in the request

```csharp
public IEnumerable<Product> GetProductsByCategoryAndQuery(List<int> categories, string query)
{
    List<Product> productsFound;
    ProductSearchTree tree = new ProductSearchTree();

    foreach (int category in categories)
    {
        if (ProductsByCategory.TryGetValue(category, out productsFound))
        {
            foreach (Product product in productsFound)
            {
                tree.AddProduct(product);
            }
        }
    }

    List<Product> matchedProducts = tree.Search(query).ToList();

    if (matchedProducts.Count > 0)
        return matchedProducts;
    else
        throw new EmptyException(
            "There are no products matching the queried categories and query"
        );
}
```

---

The results:

---

1. Filtering by category

![Filter by category](https://i.imgur.com/oFgfGl5.jpeg)

---

2. Filtering by query

![Filter by query](https://i.imgur.com/BL1bI6A.jpeg)

---

3. Filtering by query and category

![Filter by query and category](https://i.imgur.com/XCZ4R6h.jpeg)

---

# Sale reports

There is a limited amount of roles in our system, to be specific, only two... **Operator** and **Admin**, but we knew the **Admin** was the one that could be interested in have reports of sales made in a specific time, that's why we implemented the _Sales endpoint_

```csharp
[HttpGet("sales"), Authorize(Roles = "Admin")]
public async Task<IActionResult> GetSalesAsync(DateTime dateToFind)
{
    SaleBusiness saleBusiness = new SaleBusiness();

    var taskSalesOfTheDay = saleBusiness.GetSalesAsync(dateToFind);
    var taskSalesOfTheWeek = saleBusiness.GetTotalSalesAsync(dateToFind);

    await Task.WhenAll(taskSalesOfTheDay, taskSalesOfTheWeek);

    var salesOfTheDay = await taskSalesOfTheDay;
    var salesOfTheWeek = await taskSalesOfTheWeek;

    return Ok(new { SalesOfTheDay = salesOfTheDay, SalesOfTheWeek = salesOfTheWeek });
}
```

---

- The endpoint will do two things

1. Retrieve the sales of an specific date
2. Retrieve the total of sales in the week belonging to the date specified

- Finally, it will return a response containing both reports

---

```csharp
public async Task<IEnumerable<Sale>> GetSalesByDateAsync(DateTime dateToFind)
{
    List<Sale> sales = new List<Sale>();
    List<Product> products = Db.GetProducts();

    using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
    {
        await connection.OpenAsync();
            string query =
                @"USE andromeda_store;
            SELECT sale_date, purchase_amount, purchase_number, product_Id
            FROM sales, sale_line WHERE sales.id = sale_line.sale_id AND sale_date = @dateParam;";

            Dictionary<(string, decimal, string), List<string>> salesDict =
                new Dictionary<(string, decimal, string), List<string>>();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dateParam", dateToFind);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string saleDate = reader[0].ToString();
                        decimal purchaseAmount = reader.GetDecimal(1);
                        string purchaseNumber = reader.GetString(2).Trim();
                        string productId = reader.GetString(3);

                        var key = (saleDate, purchaseAmount, purchaseNumber);

                        if (!salesDict.ContainsKey(key))
                        {
                            salesDict[key] = new List<string>();
                        }

                        salesDict[key].Add(productId);
                    }

                    foreach (var kvp in salesDict)
                    {
                        string saleDate = kvp.Key.Item1;
                        decimal purchaseAmount = kvp.Key.Item2;
                        string purchaseNumber = kvp.Key.Item3;
                        List<string> productIdsStored = kvp.Value;

                        IEnumerable<Product> productsForSale = products
                            .Where(p => productIdsStored.Contains(p.Uuid.ToString().ToUpper()))
                            .ToList();

                        Sale sale = Sale.BuildForReports(
                            saleDate,
                            purchaseAmount,
                            purchaseNumber,
                            productsForSale
                        );
                        sales.Add(sale);
                    }
                }
            }
            return sales;
    }
}
```

This first method takes the specified date, for which we collect sales that were made, with their purchase amount, the sale number, and even the products belonging to the sale.

---

```csharp
public async Task<IEnumerable<KeyValuePair<string, decimal>>> GetSalesByWeekAsync(DateTime dateWeek)
Dictionary<string, decimal> salesByWeek = [];
using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
{
    await connection.OpenAsync();
        string query =
            @"USE andromeda_store;
            WITH AllDaysOfWeek AS (
                SELECT 1 AS Weekday, 'Sunday' AS DayName
                UNION SELECT 2, 'Monday'
                UNION SELECT 3, 'Tuesday'
                UNION SELECT 4, 'Wednesday'
                UNION SELECT 5, 'Thursday'
                UNION SELECT 6, 'Friday'
                UNION SELECT 7, 'Saturday'
            )
            SELECT
                ADW.DayName AS DayOfWeek,
                COALESCE(SUM(ISNULL(purchase_amount, 0)), 0) AS TotalSales
            FROM
                AllDaysOfWeek ADW
            LEFT JOIN
                sales ON ADW.Weekday = DATEPART(WEEKDAY, sale_date)
                AND sale_date BETWEEN DATEADD(DAY, -DATEPART(WEEKDAY, @selectedDate) + 1, @selectedDate)
                AND DATEADD(DAY, 7 - DATEPART(WEEKDAY, @selectedDate), @selectedDate)
            GROUP BY
                ADW.Weekday, ADW.DayName
            ORDER BY
                ADW.Weekday;";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@selectedDate", dateWeek);
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    salesByWeek.Add(reader.GetString(0), reader.GetDecimal(1));
                }
            }
        }
        return salesByWeek;
}
```

For the second method, we make a dataset, in which for each day, we display the total amount of sales, so that we can know which day has the most sales in a week.

---

The logic of these reports can only be really understood when taking a look at how they were displayed in the web client, for the admins to take a look at them

- After sucessfully login into the system and clicking in the **Reports** section, this is what the admin could be seeing:

---

![Multiple sales in one day](https://i.imgur.com/0SHGIYU.jpeg)

---

![One sale in one day](https://i.imgur.com/TUV41WP.jpeg)

---

![One sale in another day](https://i.imgur.com/5atGQf8.jpeg)

---

This was possible by using the Google Charts library for React

```typescript
import { Chart } from "react-google-charts";
<h2>Sales Chart</h2>
<Chart
  width={"700px"}
  height={"200px"}
  chartType="Table"
  loader={
    <div className="progress">
      <div
        className="progress-bar progress-bar-striped progress-bar-animated"
        role="progressbar"
        aria-valuenow={75}
        aria-valuemin={0}
        aria-valuemax={100}
        style={{ width: "100%" }}
      ></div>
    </div>
  }
  data={salesData}
  options={{
    showRowNumber: true,
    cssClassNames: {
      tableRow: "chart-row",
      headerRow: "chart-header-row",
      tableCell: "chart-cell",
    },
    allowHtml: true,
    page: true,
    pagingButtons: "both",
  }}
/>;
```

```typescript
<h2>Weekly Sales Pie Chart</h2>
<Chart
    width={'400px'}
    height={'300px'}
    chartType="PieChart"
    loader={
        <div className="progress">
        <div className="progress-bar progress-bar-striped progress-bar-animated"
            role="progressbar" aria-valuenow={75}
            aria-valuemin={0}
            aria-valuemax={100}
            style={{ width: '100%' }}
            ></div>
    </div>
    }
    data={weeklySalesData}
    options={{
        title: 'Weekly Sales',
        sliceVisibilityThreshold: 0
    }}
/>
```

---
