# Andromeda Store

Andromeda Store aims to be an E-Commerce for everyone interested in shopping online electronic devices, whether it is finding a new computer, a new keyboard, or even a colorful mousepad for your new gaming set up.

## Composed of

- SQL Server Database
- C# API
- NextJs Web Client

---

# Content

1. [Architecture](#architecture-house) :house:
2. [Diagrams](#diagrams-bar_chart) :bar_chart:
   - [API Diagram](#api-diagram-globe_with_meridians) :globe_with_meridians:
   - [Core Diagram](#core-diagram-wrench) :wrench:
   - [Database and Expections Diagrams](#database-and-exceptions-diagrams-file_cabinet) :file_cabinet:
   - [Models Diagrams Part 1](#models-diagrams-part-1-memo) :memo:
   - [Models Diagrams Part 2](#models-diagrams-part-2-memo) :memo:
   - [Tests Diagrams](#tests-diagrams-test_tube) :test_tube:
   - [Services Diagram](#services-diagrams-package) :package:
   - [Package Diagram](#package-diagram-package) :package:
3. [Sequence and Activity Diagrams](#sequence-and-activity-diagrams-repeat) :repeat:
   - [Sale Case](#sale-case-money_with_wings) :money_with_wings:
   - [Searching for a product by category/categorieies and specifying a query](#searching-for-a-product-by-categorycategories-and-specifying-a-query-mag) :mag:
   - [WebSocket interactions](#websocket-interactions-globe_with_meridians) :globe_with_meridians:
   - [Payment Methods Activity Diagram](#payment-methods-activity-diagram-credit_card) :credit_card:
   - [Carousel Activity Diagram](#carousel-activity-diagram-carousel_horse) :carousel_horse:
4. [UML Diagrams](#uml-diagrams-triangular_ruler) :triangular_ruler:
   - [Payment Methods UML Diagram](#payment-methods-uml-diagram-credit_card) :credit_card:
   - [Multiple Existence Of Products In Cart UML Diagram](#multiple-existence-of-products-in-cart-uml-diagram-shopping_cart) :shopping_cart:
5. [Security](#security-lock) :lock:
6. [Cache of Products](#cache-of-products-card_file_box) :card_file_box:
7. [Product Search](#product-search-mag) :mag:
8. [Sale Reports](#sale-reports-chart_with_upwards_trend) :chart_with_upwards_trend:
9. [Application Walkthrough](#application-walkthrough-tv) :tv:

---

# Architecture :house:

The project follows a 3-tier architecture for the backend, splitting the solution into the following tiers:

- **API tier:** Handles the requests from the web client. Some requests may require authentication to be authorized, while others are public endpoints. The security aspects are detailed later in this document.

- **Core tier:** Validates the data that is received and returned, ensuring the proper handling of information.

- **Data tier:** Manages data persistence by accessing the repository, writing, and reading data.

---

# Diagrams :bar_chart:

In this section, you will find diagrams for each tier of the backend solution, as well as some important packages.

---

## API Diagram :globe_with_meridians:

This tier holds the multiple controllers and their endpoints for facilitating communication through HTTP requests.

![Api Diagram](https://www.plantuml.com/plantuml/dpng/bLJ1Rjim3BtxAtICaUsyIn3KSveXG8OLgRlBi0DZ4QuGMV94Ue1HzDzdHLPIny7Qw29QV8zyJo9yHmoEsredIGFb0IeKMzX_4Ol53ai6KsbSBiLB8aHCPuswZl3hXCgjOMUrHaSzScsrH1TXmoer48aS79_18u1VymBeiDXa9IjhTaYjvgN87GAZPsRKcNBc8r62u_GD-tNIwX4qdbjzgDCtPC-Yq6S5Z8MgKKWVsBKoChseylclLLBy1AqynXCkMJ7tZbQsxdzYZ_cZF1_CqilyvjwqDJhOQrmyE2lRChWPOig1iR9E8SssYdYXJ9ylJuUV1B5JfX9_MdJTM6eGcBNyVEDbRcsbJBZ9NsjdwwzMTh_54w7pnKJR1vFhV0momeiNze0cwCqrgBfNx3UwfY-7kW5LcHirXaQHF14R4w8SWlGZHLUeyRBeO4p9D0cSjoyMeSlZcyKl2inhRX-Wgz7mT-HdAs-q_DJuSHnLcFWUPNVOJmMnS65Faz6qZQ1qD91fFJvzrqFiyPBCbU6OAhe6JyJvtbejDdHlV5kPn3B3XDzu2PcK0t7gFPolYX-Y2M4wbd_UNtiLTsYa_v_-0m00)

---

## Core Diagram :wrench:

This tier handles validations and most of the business logic for the system's workflow.

![Core](https://www.plantuml.com/plantuml/dpng/fLLHRzem47xth_1Zgj2zxc4eAeDsciPe1T7UZ_WMh3fEPZlT4Ef_dnt7n26qBnMIyTtttVc-yndkj05bsagG1efdA96kOV-PpkTRB0p8Kc2MqTCPNDOAn-20VMqrbwYrnHzgmL0vi11WFKjxnXa-aLjdhy00BVeDIR_VoxP21Nk1yoTLixOm6UqtcgHnXkI6f3dev_aE15fIgue3Q5peeomIH-pItf3Rbvep-Wi4Ps3G0M-ZJt2iK9gVQ0urIqQMfariNaQw8XAtK6qNYsgZk2pfDvIeB1xAsxJL7fMj_RLlYei9acnFdDdrH3EIEcCYDsgHbvxPGupRQQ_TXcFEAwJCRUe7Bjcrs1zujCMtLZDNSr_jZ38iU0KYyqdpse1uD_Ea3uvooGlO83fNY9kQuOca_UM6IoPfmEWQVyVcbfU7o4v2OEE_X-Gzi4EmBNTqszOGHLLTcoyaxMfUCDPpfyc6Y8XrzacxpEWqwEhu-aAIVY3-jAYEeMiUkimRCsTqpRKPabLWYWE6YlIWUWbL0xoKqIm5btzZaK7I04scAm2PqnqCV_ixNvR1VsRAtc9Ll-3L07vr_0BLJrgm-a4RP8tVO2HkjQTiJz8f7ikXzz8TDYBv4yPnd_9ZWxI9VSuJigou_c2QMFauffE_46BixJemdkT8Ut1r5ziCUyAbGIL1K8O2Ip18kp5xb1ly--Qedz_WPVU6gPk-b5N-639CVaJVORvsdpkKpFtP_0S0)

---

## Database and Exceptions Diagrams :file_cabinet:

This tier is responsible for writing and reading data to ensure the correct functionality of our system.

![Database and Exceptions Diagrams](https://www.plantuml.com/plantuml/dpng/ZLHDZzCm4BtdL_ZOQdUvSY14IviMhIWVeWNE4sVKjUeumJCfL5N_E_OwJfFT852askDvxyt7sBqd1ijjfPC6v07sA3PGl1Pf-XKbWzbhp39nxfqvC3ZdkjObsYmx9r83aLWw3Uyw9pSxe8FOmX7VqSd8sHOq2d8_y-JIWVF2mOYjCdktNzR6e6HLc-tJKN9t9WPMyf8Nui6ulP7u9hc9fyTQbMBHAbtcnQnJePzwY_Q8zhdOFBdxBrIl7XRn56Hf4HZpnMp-BFfQQVq5JXKQ_eZyM9Sq3R5rsKhspgOjj9FTAE9qpCp4U-I_YmL640hGgsXezRUc1CQHGkZyM5GqG_Cw33-VCBbn0QxpQDbx-r3fmygq5LeeDAOVyFGTTEi2Avk61j-A4gMgG6UPJpMG5wSVY8UGaOk3ErMXy1dxuobXpnhnFMk2lwlNoWmAMKBLWDgRuJKCHKHF48cMaDqc9X4TcSXH8-E85k-GAgToZj2GTxGeCFzDs03nxj4Ylc1UagwwRko3ugwt8bJPcO7P6Q89wstaBrfI1ebUVJQVy5T4rKFZhJ2jrYwq-_fdl_ejiV4FXYOU_wfg-DGZpl6oZuzZ0oiaSehNkSMKNWgCFTCQuRj7K_g_i3y0)

---

## Models Diagrams Part 1 :memo:

![Models Diagrams Part 1](https://www.plantuml.com/plantuml/dpng/XLJDZjiy3BphANXqu2_VjPVKtV_2mEuss6qVGB7Oh51PCYHv2yFDkvUo8aTk3JIvc3FaYEBGldAUMz-tYdMy-S6F28_yy05sknTiFDT7XLK5uqm-6O7A4VrWb41RLIDh57SERabbP42_yb4wlmlntXhHDxw2yqCjNCnmtafzX6iXBBepjkT3Yze_eNypmlt_TUWG5jYYzDRexzAst4kZd_lsW9QT675q2hNYyMZiCB9IQW-rOAdgcRV8jeal0Yl5Vt1cDR4RLexEasnpIcbmqqibrfDFwVOnLw8RsJONY218JPXkwF1fE4cwWcREOULANJtNNS1YmxRlHehOrmfEO_t75UjxtRTe-K5Xrijdz0krCb4PCzmCTPf5k3u_a3AdeI_jez4Z7G71HLRcNaRaO8o2sjthq0BvC_KT8peuXdVIfNWfNgojIZVBvnukC-51Qh5QxOSkS_DLwWup8oDmCR5S4fbBKrpaU-7USg4Gvpevdic4C5Fvvj1UzvwKojTiaLEmfvoVneePU36As5HLPDxFYz2TQnBm2ImLmKVGlL8qm29sje79YjhjBJhKlaYYqDD3Ta-BWcZ9rPSkl99kN2vPAglW3jzHcIuumCg_oehD6hYk92vAeP4_hDxzscwdiQuHqJV6iZVD49fsd9teVuLQXC_YRm00)

---

## Models Diagrams Part 2 :memo:

![Models Diagrams Part 2](https://www.plantuml.com/plantuml/dpng/nLHVJzim47_tf_1ZKTBdlLGHq0vK2HYmyW4S-rQyENRcFzEggDzzJh2Jk6a724rJfSPtvxlVx_xupeqbshfAe9hG7sGB-8QKd_1YyGZK4haLK1IuwOstYe4mtdob10DT50sYWXY3xxLYZje6PSPgBhVuZbJG2-lAEpzfWJ86b5T4-FkS3lOL6AfvRRcIABjsdE4d_uUo9R6mLNg7um5bglpkcU6bK19cPsYVugysNpvEuIXk0C0R5KuZy2NHDXOWy5-p2H2N5Zyu8YstkqFUfa7pnbXYETttEiobLqdlEx_XnYw2li1rk83cAqvRnaJl5ZvyZfDxHH_fSjSdCeywpp-RGgvxo5_Aft75XHWSWlA2iLY3y8sCVCZrP-agqAGKC12-1ZkbCkl8Skdjz4N5mRmfmeC3xnuQzxCLtWLy8PcFS890ZjyPjvk3qCslH41pmYNUJysZqIdmI33zOFfgQp1c6AUBIZbf_KIINGNIte9zLipWHEozxvscpyJ0dQjAqBrwgUGthYlI3azgwd9O4Uide3tDJfAb8x9vy2KlNFD8bNHKymEczLYChdL2D6hfXETPu1KkCCyqnmTu-7XCFCfb-fZ6pw55m9UE2tOw__f_vDvJ71XUAVq8jTBM93L9QPn0UgKW-pZMLkbshjCj59k_bgqzBBWD-NqFceAqP0lH8oc11qXab2LzYO-qmE748IAC6jH2BPtMFaoAE6pHLObPENBfGepsMoVFZY_MV99A_ePCykDvlBxS_icA-k32_VksJQlQhS3sTmwIkKgWFm00)

---

## Tests Diagrams :test_tube:

![Tests Diagrams](https://www.plantuml.com/plantuml/dpng/fPPFRzf04CNl-HG-jgfwh7f1KHmW3MhYe02Dr4iqx0xsYlMkDJCcq2ZVlJRnHdM3vLeL4XBhztkpy_QFkM01aZAtPmMeBQGOtS3wQpGQtQCISAd5y_Fe-UtXQldkaRB078r1CFLqM28BFvzzsdcZemNAglZmiVaL90z6ifd-YUGVbndv9uufBNDqCjqhBCHuTn99V08v3a9guAgqzcxpa1d1HTK5yZyxN0F_06jqKFBZn2CdNleCvkHrgOGl3y6gXuoThee5hF9vQLATbwGoO6nINOWdZBZ-xaYOgX8WbLdYWrrVlsqgrfgG1q9pEDIgMvJCwm7ey7oNmDje0HO7jDbG8Skw_74BEb1YTdXgyfSb6uVCqts1whX2V72gqsfXN86l1h7JYHzxjp6KGqqcPRv6wlDzENsCFjzwjHtDxewTy6XCgD690TkCL0TG_JrM8m6yzYnJjpFaNTtZUPIr1sfLUoU5Y4k5p9lIshYKhFPNnsxkKKfoVFVzJNdbQMsqHdTIk78WcIVp2tMhoxm0axfmMRHhZmb1C6XUZ_uIzz9as5hP3g2Mtm2bICiCt9VjE0C29KZSQNAFkT-rJAPv8OUP7bWsRE8kwXjAR6sWk2N_KnsoQctFK_T0-mgO68QrnHRLKICWK_TVHEBTT6zO0dmoi32tOLpOOYWgCovjJR4_kN0pz8SMQrtP9_ZK8FLQ-bB2--PzCztCB8SKLsI7SXDaHUOe7OhEoIWChzCXOBXi1hDZJuHA9Y3GlGh7pmKwNV-v-0q0)

---

## Services Diagrams :package:

![Services Diagrams](https://www.plantuml.com/plantuml/dpng/RL4zQyCm4DtrAswC2Pwx694s9H18boQqepbBXn6MfQ2x98F9Vw_hA4vRYWRnFlJU0wrOCCcfz-g8fiEMO8VD4rJLEnd1q7hI6ePPt5CwEqCy6ZRHMqfQ3yfuP8PdQnCnVw1t5YMcGIqNB2ZE5D34w24hb8sRc7gKqP9SQ05lPA6kEUwJcdqq7Kck75JvYw84DK-tAdUEfThEyZg6CAvtCMYeB_o0hCerzaTqRNX5GQZD3wIMVollsqx95sfvZix2Iufe3RAy4JEsiwM_mKAL1-GEzXJiDZXnwE-5lFXl85muHqzlScfrq8l38_CwdTMeV__J5m00)

---

## Package Diagram :package:

![Package Diagram](https://www.plantuml.com/plantuml/dpng/VLBHRi8m37pdA_YG2i60HyQqiktP9ASs8aqgsvs69lvzwzQKjgfGN-ppNNq-PIrAh4tb5pMP2nLORYCZorvXb4BXiLejl_lPKoCkG2JB4l8tDfv4bbkgQd95IAGUPfr0BtIj4FG0BQELwP2ZRSnKafF7F347iIbF37HuMpv7YyMjzxqZfOUUMyB0xw3TdKVhqYXvQjl1elsNGQqkXlc8zbMjrvxJWUciEtx_to5Q-CTn3sptfQA8V1st3lFnfsDSgD4hf1nVFUva4idfE30nLhGPfL8Zf_fDm9j6owHZM0Hrv8__wGqYoC6Vpc0-X8sr395tyiwIHkxW3vppQ2xGJhou_Nvh1Di-z1y0)

---

# Sequence and Activity Diagrams :repeat:

## **Sale Case** :money_with_wings:

![Sequence Diagram](https://www.plantuml.com/plantuml/dpng/bLF1ZXCn3BtFL_W7-84Uq5PLGI21Lgt45Rc9LMNbYMSTJyNoHnmuSUFQ7oFPAQEPKKhfPTGqVi_FyNiFoL2jRxb1PwAmuK3HgEbGBRZGOJJuJD_iO_ewNCrkrbsOdJUYr7WqF60Ys3_pdlH4sZGNsZTlvsIhZ8YfPqFeL7oV9QIcA0adD5eqRcR7J9Mxh-0z6UpekQTau0aUApH5PPC_6L84lc8kTPEoGNnJlhbmrBw2lN2Upn0u94ClfOs9bmGigQPxElZuSywvVIBN6qBMhI_N2AVOGRYIYKQ43zlOjwHuO5eCxQcceg271xkJOtYJZ5jkxawUpB3O7sJ6YojMq9RrwSG4G16Aev0P1wAg9QxxQdrKEau9aS6XQZ2v2V91oRbm_XNBGFcc_7iJrPv_c0pkwF3uZouxGWxV4HZ1ITif_gVZuUy67sNm-cQ2hXjz6q_ddmXoU28JsA1QrUzZ_HVauD45YPGgIPZJxP7fBjElkKL7SJ9vnUBlGbmyppLJ--8PULdwV9BoI8hGHuZdtotfwsgKN3Z8B12xYv69AuNBC8nThoRZ3iAC7iUeH6IQa4z3GkgS8yV2FWyKVT_o7m00)

---

## **Searching for a product by category/categories and specifying a query** :mag:

![Search Diagram](https://www.plantuml.com/plantuml/dpng/fP9DZjGm48NtFaLT0B7l1Heq6X1884HBR55DN7GREQwCNHxHHsB1AUPYb3jM55iPK60JnJ_rlLUkTvC4e-JH6no48znwHq78JBhh1ZTX4FX8F-Hz-ZeVDIUl9zUiRpcIiIXuZudW-E2F51yf6bEnBrwri8DMX9Iz84oHRLOBoHGdxX65Ec7JB1MbwWTuImAVwI5J4h04dpOmnQNHZvOKWzUQUczrRJPlobSlBjuFS6Il_Gc2TqdGSf4Hjfp0SzhmtJI-_BRCk-yqP4MgxtXv1jZKpiOZ2SU0yEukv94YtdlgchQqvS9IaiYNzLO1Bjm1XpDXAmyKOAIHeyFbjZxYJXFpesfTF_gT72Lq0G0brbWTOCobGspTbqYYzzY06iQkOC8tvrKZM8Q98mnAEt5y-eMW4tGd9zUZ7EQX_bUCLw8JHWIFBpscbMI2Iokp5VCskF-ABQmkBb_GE_jCEVbAQ4lUbBVh4sRfchoddtyFsC1XLQWASCOJQa9HXlD6uchYT-PlXumY5vMDTAx3-Q66KvlluJeVYbpvyr1Iha6p_AVWLkb--uQ2pQF_3G00)

---

## **WebSocket interactions** :globe_with_meridians:

![WebScoket Diagram](https://www.plantuml.com/plantuml/dpng/lPN1ZX9148RlynJr0hu01xCuwm4FgrcIzMYAxf9KdEaUguh9hk_amNZpoelP3QETGM0x7lOmX0xzVtyL_q-uKKEnNTyrw2mAj1rJC9fEIzzpQ8PqWnqF60pkwD7UwQVZjTadom-hsRcDGer7mmqgmVfhjoOPIPfcidZrUWw31Jomii60suC_ZsXqOZXN94HoNIIhG8-y_ndW8stMqNqXAu0yLthItJnHSdc_CGeaiF_L6VUeO48A3li1zpzGY_AmGNv9HcK1EFZfhZE48Px1u2cdx5uWPPnxiYW1BtCydI5byRH_DQ8ibLHdllwtOTn_hwCamBzPJC4ku9PqIE-tTQnvmisXSt1cl5LWnn4yWPBcj2-Lv2AX5KBvc_IbCWcXKPilyJOizIcuAsLwm8vzWZx1GAAiniMWeache2I6HLFRcdQw3tmaU7o-9qNqPrQFbTMuYyQVsQ40WaKV5Rhqk6FW2YDZ-UazLmpQ9apOqOXbwtcrhfJWJHH9ylPwXhVKqVzasAFa7LtiSy_qAF4rGIwTxHBc6qff-ILEVRWuKUlZdSjUEjzxwkD8fSspvKyplg7Wyn_FRm00)

## Payment Methods Activity Diagram :credit_card:

![Payment Methods Activity Diagram](https://www.plantuml.com/plantuml/dpng/fLR1Rjim3BtdA_W3m-u1CjJ8jc83CXGDrkl0I6pAGPOyYYws_KsFE-oqswxviKLk0jLsQwNT8G4ayJtIV4-qpwAYQ5kx6HeD0fzZYyBXiAfipPwZ2jeWimP5sN23Nk4JVTUFySlICNdjdLGDzzRB83IpgBZ1IB3-vjOajoIpsMAnpqO25wsO6umKuKA2ROr6M2ncXqBWrPj-hZbKMw4jGdCV72AW0uCYhA6CiYna3E_-UB04v3Njbc7rxfS66nAcmUrKdaj2npyH7889TIEOMictg3JeMM-LYAg6vt0HegOiom4sTNN_It6feNDuvsztln72vYjfW2MAvk2kvUalXr-XOuk6WwTuFCfYxCz9-9eDGcGWDtnUeAYxExZ13JjMj27doeAYKrWDS9RY0n00vFXSsoLf49-RF0PP6kTB1nt1eNUPXqI2WAN1KnI84rNQIrnZNV-FF75sMoFH3qQQmwgbgF92_e6ro4KQIL7MuOXuPlFpxj2LQtJtBNsS8Ay6B9MhqP1_S8cUAlaz--5rAvQSqUVaIH1Q3txtjoRfcYLaU1CctEN6qI_rKfdgTEUKsSO-yRORmtdCQn1OeSTjciRueqwvLgGtmNP3kVS-UEJ1FdWsZ10ffWbR3B_Yo8hGxEwsxF61wUFHjKzCoNFIohb3WOVw9cRbEZWsh3YcxoisDdG1Ed1OURPb-ACMxHck0CrbJvYi0DyZnntkgPRjDU8fobjrQLw_fNYVS3ZIdlLcJ6njkkiJD0FXfp05uIUWG1us62cjBMg89pfX6VmrIus0H-RkCwHeX0Aou8C2-dGRb6a6AfwHj-cxyXy0)

## Carousel Activity Diagram :carousel_horse:

![Carousel Products Diagram](https://www.plantuml.com/plantuml/dpng/hLNHRjim37ptL_07yWC1ChJ8C61x68e5xUl0Mcn3m9OqYNBMlvyLLORL28Ozz251XF3ko3lAllE2JaBVDTYASV3e0pesZKKdtB95BV2J_ieF__lOCMaf_hctN9oFnb6ZKF0PFS7fJtSYDv1hciC1dfWkh5y1jOA7uDep-dXyS4Q5LZmS3atphWwxBwNe_ee6-ruRU-K1XJwqLfn6ajZV7dwHEAQ1CXoSkSmeuWpZ9vR7zlVmZQGzrulJWF4BTZFwu7HPhwX09FxReRwojns-LR6vjG8IIN94UpYTpQMElN4c1d2lL66u63YYasNZ8shIU76cXnQT2PwwJI4KD68YYIn4CFROalVNCa2bMDZeDIa4gr189Q0NbB0wZAFHB-pwtEKako6Gnwjw4bSfsyL8qfnqZUN2SixtWfOpoR1e92hbebCMNsbR8gVmtBDaWaeOjLr8OWDsh725EJDlygxZ3GUCvz39WbEhdB3uraVH_tOY4_GaPwDM6VAKXg0FsCskPFmdkJAjSbgdZNPyromSYpCSRBgEAoviXbJlQYwUN4lNMwiy_bGtlJd6S72OTQ98aBjP9rDbC_s2NLRAHkHLhQ3gp_8irvRuRG_rEicsftiTUxCATwHLV8V_0m00)

---

# UML Diagrams :triangular_ruler:

## Payment Methods UML Diagram :credit_card:

![Payment Methods Diagram](https://www.plantuml.com/plantuml/dpng/jLN1Rjim3BtdAmIVaYqUi6iG6CkQTZ2m0a6MVe1i4vbNHV8a6bXGzDz7nPuJAchWG-T3w_0zqe-aI7rnA2ss1pLfPFaizo2IrION6sj-GOc9M2wtz5_glO8i4oyJGS_0l3CQhL4AhFFC1wCgi0FDFwMIpeaVQ2oSoHq-F1-_0PuOqvbOTDm1Vvq4eJRoU02DZu0_JVLco9JHtDVMrHh8kucQkPgdff88BC3A7NKvjV2x1OUaaO7RpkorxwHxNkQh4ckZj-1QXTbLfYkbkBXJrlczRWzWPQ5WoU7ieXRTNwh9HGSiZ6pMewb0ZUXJs9tljSCbVQ9gIyn4_-9oVI9pQgsHQl1d0xO4ZVJLM2IUNT0Ixd1PbJk9i3Vsc8d-hOQu28SIwxBF9TVqKvUSalQKTI6gWY4l7VBQUK-RFbbg4Al2d6T9C6K-3r8Pd9YLVCj1Fr6YASvkYPgD70uNDXBeIs9tR82RlUMVvpLQ6AD4xkwrLrXnh3DEPzowhXqpixQ44XzgNKtHArW4q2UlPNPbZYJVjphvd8Wq5Sc79E92T3uD_UpVdeVu5WbtpN2CWih4TWWx8hkRUVjb4gdikuHz_wN4gxkMA6zMTbs4ftEOIJQpJ6L4Ocp2ngrpNhEhUh5IyUtx7s_AwCr2f73BZftNcFhmCaxJh9-WXKZELt4oOLe2X_Xa-0XF3Xpvnd7lkYv6qJXqTY7tE1mv87FnPlSflxy0)

---

## Multiple Existence Of Products In Cart UML Diagram :shopping_cart:

![Multiple Existences Of Products In Cart Diagram](https://www.plantuml.com/plantuml/dpng/ZLLTJzim57tlhx3wb0gw7p1L4Q4RADAOEc1xD_PTyN2SoX-JAiH_txsD5spJ4lBGf3x7r-SSvpeNpdFhGwTdEowU-1ROlDcqR6FxFo3ydArMjtZdPgkXhjdpZE4rCjUzyRRN6gmZvbMl9TYHHfVGt3csnYLUkGbCrzdQ0lT0dCRjZQW4Fh7FXqaBlDzpzxHg6-5LRsx11UthSVxBRFZDPTtq4lIqebB7D-NyYW0qBuFmDOiFhNGvqtchp9OrKbfm1RRX-mwClm7_s4ltwNw_0vQD7Is5wVnMjkDaydle7i0c7WljYSJImdLGahN54XA4whX6Dqf03YdZsO_0ZLT-Vth1EwvXEgpapuVYoiKrNH-CVo_A3wIu2LOySWT5WjD1ZzUpymY9by7pPL1QLhje2j_5zgi97LZ-e24pVyxuO0uvmq8uSd240z7YETkb1X38_I6UoKzAYKnq1bA7aDQ9bhWCJXbmqqtndrZizvAmBznp9k93dcuQJDa-POD4bwURdVpK-VA_k5OodXBPdEEqBDdg_TnFb20HbSEV6711kmO35cdvArSjnesPs12AS78pY5GstXt_6wDBnSNeZYuQ2-Qkax9dW7l7wA1w7KgFhHmP6gIiTgMCh3mIYtZZ2RXuCnpreSBYux1S3aS9pfWFNmOtFt0YCZOT4MWULmOPsRkRra2sm34umYbD8Fjd9CRiYKJv4eJ14tO1HjBdyXy0)

---

# Security :lock:

Andromeda Store's security approach is straightforward, prioritizing simplicity over robust authentication and authorization processes. Notably, the system lacks a user module, which means there are no user accounts, new registrations, or password recovery mechanisms. Instead, security revolves around roles assigned to users. For authorized data exchanges, a token is generated upon successful login from our backend. Let's delve into the details.

---

Firstly, since the system lacks user orientation, verified accounts are stored in the **appsettings.json** file of the **API** project. The user credentials are structured as follows:

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

With our improvised account database set up, the next step was configuring security. We chose **JWT** (JSON Web Token) as our token provider due to its flexibility and widespread support in the market. Here's how it was configured:

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

In the provided code snippet, we validate the issuer, audience, and token lifetime. Both the issuer and audience have static expected values, reflecting our simplified approach. The symmetric security key, although crucial, was not customized beyond a default value found in online examples.

---

But now, what do we do with this configuration once a login is attempted? Let's take a look at the dedicated endpoint for such action:

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

**Explanation:**

After validating the provided login data, we check our **appsettings.json** to find the user. If the user is found, we create a list of claims to generate the token. The token creation process is as follows:

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

Using the default key, the token credentials are generated, including the issuer and audience expected by **JWT**, and the token's lifetime (5 minutes based on UTC time) to avoid issues related to server time.

---

Finally, for endpoints requiring authorization, the token must include a role. This ensures validation of both the requester and the token's lifetime. Authorization is implemented as follows:

```csharp
[ApiController]
[Route("api/[controller]"), Authorize(Roles = "Admin")]
```

---

This meant that our web client had to store the token so that it could be easily retrieved and attached to requests when needed. For this purpose, we decided to use **Session Storage** to store the token and its expiration date.

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

**Explanation:**

- **Storing the Token**: Upon receiving the token from the API response, it is stored in `sessionStorage`, along with its decoded expiration date (`exp`).
- **Session Management**: Any existing tokens and their expiration dates are first removed to ensure only the latest token is stored.

---

When required, the token needs to be included in requests made to endpoints, like this:

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

**Explanation:**

- **Including Token in Request**: Retrieves the token from `sessionStorage` and attaches it to the request headers using the `Bearer` scheme for authorization.

---

Simultaneously, the web client checks the token's expiration date. If the token is expired, the authenticated user must log in again to obtain a new token.

```typescript
function checkTokenStatus() {
  var token = sessionStorage.getItem("sessionToken");
  var expiracyDate = sessionStorage.getItem("expiracyToken");
  var isTokenAlive = checkTokenDate(Number(expiracyDate));
  if (!isTokenAlive || token == null) router.push("/Admin");
  else return true;
}
```

**Explanation:**

- **Token Expiry Check**: Retrieves the token and its expiry date from `sessionStorage` and checks if the token is still valid using a `checkTokenDate` function.
- **Redirect on Token Expiry**: If the token is expired or missing, redirects the user to the `/Admin` page to log in again.

---

# Cache of Products :card_file_box:

Given the potential for high traffic and frequent client interactions, we implemented a caching strategy to minimize database queries. Alongside using a static instance of the store class, we introduced a static cache of products represented by:

```csharp
public List<Product> ProductsInStore { get; private set; }
```

This list contains the entire catalog of products available for purchase. After the initial query to the database, subsequent client requests access the product catalog directly from the system's cache.

**Explanation:**

- **Static Product Cache**: Holds all available products to reduce database access frequency.
- **Improved Performance**: Ensures that subsequent requests for the product catalog are served directly from the cache, optimizing response times.

---

To illustrate, here's how we return the product catalog to the web client:

Firstly, the client makes a request to retrieve the store:

```csharp
[HttpGet]
[AllowAnonymous]
public Store GetStore()
{
    return Store.Instance;
}
```

**Explanation:**

- **GetStore Endpoint**: Responds to HTTP GET requests from clients, returning the static instance of the store which includes the cached product catalog.
- **AllowAnonymous Attribute**: Allows unrestricted access to this endpoint, ensuring that clients can fetch the store and its products without authentication.

---

After the client request, our store instance is created by retrieving data from the repository:

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

**Explanation:**

- **Initialization of Store Instance**: Upon initialization, the store instance fetches necessary data from the repository (`Db`).
- **Data Caching**: Populates properties such as `ProductsInStore`, `TaxPercentage`, `PaymentMethods`, and `CategoriesInStore` with data retrieved from the repository.
- **Static Initialization**: Ensures that subsequent accesses to `Store.Instance` do not re-access the repository, serving data directly from memory.

**Benefits:**

- **Improved Performance**: By initializing and caching data during application startup, we minimize database queries during runtime, enhancing response times.
- **Data Consistency**: Ensures that all instances of Store across the application share the same data, promoting consistency in product catalogs, tax rates, payment methods, and categories.

---

# Product Search :mag:

To empower users of the store to search products by name, description, or category, we leveraged our product cache. However, ensuring swift response times for user queries necessitated the use of a secondary product cache.

**Explanation:**

Initial System Startup: Upon system initialization, a primary list stores all available products, initially populated from the repository. Even before products are written, our secondary cache is primed for use.

---

**Initialization and Data Population**: During system startup, the `FillProducts` method is invoked.

```csharp
public static void FillProducts()
{
    List<Product> products = Products.Instance.GetProducts().ToList();
    // Additional code to insert data into the database...
}
```

---

**Accessing Products Instance**: The `Products` instance triggers its internal method to populate products.

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

From an array containing 40 products, each product is iteratively added to the cache:

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

**Explanation:**

**Iterative Addition**: Each product from a predefined array (`productsData`) is assigned random category indices and added to the cache using the `AddProduct` method.

---

**Dictionary-Based Storage**: Products are stored using a dictionary, where each category in the catalog has a corresponding list of products belonging exclusively to that category. This ensures efficient categorization and retrieval of products.

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

**Details:**

**Efficient Categorization**: Utilizes a dictionary (`ProductsByCategory`) to organize products by their respective category IDs, ensuring quick access and retrieval based on category filters.

---

- **User Search Options:**

1. **Filtering by Categories:**

```csharp
Store.Instance.ProductsByCategory(categories);
```

2. **Filtering by Specific Query:**

```csharp
Store.Instance.ProductsByQuery(query);
```

3. **Combining Categories and Query Filters:**

```csharp
Store.Instance.ProductsByCategoryAndQuery(categories, query);
```

**Functionality:**

**Flexible Search Methods**: Provides users with versatile search options, enabling filtering by categories, specific queries, or a combination of both for targeted product retrieval.

---

**Searching Products by Category**

- Retrieves a list of products assigned to specified categories from the cache:

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

**Explanation:**

**Category Filtering**: Iterates through the provided list of category IDs (`categories`) and retrieves products from the cache (`ProductsByCategory`) that belong to those categories.

---

**Using Binary Search Tree for Query Search**

- Implements a Binary Search Tree (BST) to efficiently search products by name or description:

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

**Details:**

- **Binary Search Tree (BST)**: Ensures logarithmic time complexity (`O(log n)`) for insertion and search operations based on product names.

- **Searching**: Implements recursive search (`Search method`) to traverse the tree and find products matching the specified query (`query`).

---

**Handling Mixed Category and Query Searches**

- Combines category filtering and binary search tree for optimized product search:

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

**Explanation:**

- **Combination of Filters**: Utilizes both category IDs (`categories`) and a textual query (`query`) to narrow down and retrieve products that match both criteria efficiently.
- **Optimized Search**: Uses the binary search tree (`ProductSearchTree`) to perform quick searches based on the query text, ensuring fast retrieval of matching products.

---

The results:

---

1. **Filtering by Categories:**

![Filter by category](https://i.imgur.com/oFgfGl5.jpeg)

---

2. **Filtering by Specific Query:**

![Filter by query](https://i.imgur.com/BL1bI6A.jpeg)

---

3. **Combining Categories and Query Filters:**

![Filter by query and category](https://i.imgur.com/XCZ4R6h.jpeg)

---

# Sale Reports :chart_with_upwards_trend:

Implements an endpoint accessible only to users with the "Admin" role to retrieve sales reports:

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

**Explanation:**

- **Authorization**: Restricts access to users with the "Admin" role only (`[Authorize(Roles = "Admin")]`).
- **Async Operations**: Executes asynchronous tasks to fetch sales data for the specified date and the total sales for the week containing that date.
- **Business Logic**: Utilizes a SaleBusiness instance (`saleBusiness`) to handle business logic for fetching sales data.
- **Parallel Execution**: Uses `Task.WhenAll` to execute both async tasks concurrently for efficiency.
- **Response**: Combines results from both tasks into a single response object containing `SalesOfTheDay` and `SalesOfTheWeek`.

---

The `SaleBusiness` class would handle the actual data retrieval and processing operations, ensuring separation of concerns and maintainability.

```csharp
public async Task<IEnumerable<Sale>> GetSalesAsync(DateTime dateToFind)
{
    return await sd.GetSalesByDateAsync(dateToFind);
}

public async Task<IEnumerable<KeyValuePair<string, decimal>>> GetTotalSalesAsync(DateTime dateToFind)
{
    return await sd.GetSalesByWeekAsync(dateToFind);
}
```

**Explanation:**

- **Separation of Concerns**: `SaleBusiness` encapsulates business logic for interacting with sales data, abstracting away data access details.
- **Async Methods**: Provides async methods (`GetSalesAsync`, `GetTotalSalesAsync`) to fetch specific sales data based on provided date parameters.
- **Repository Integration**: Presumes integration with SaleRepository or similar classes to interact with the underlying data storage (e.g., database).

---

Here's the final part detailing how the sales reports were displayed in the web client using Google Charts library for React:

---

**Displaying Multiple Sales in One Day**

![Multiple sales in one day](https://i.imgur.com/0SHGIYU.jpeg)

---

**Displaying One Sale in One Day**

![One sale in one day](https://i.imgur.com/TUV41WP.jpeg)

---

**Displaying One Sale in Another Day**

![One sale in another day](https://i.imgur.com/5atGQf8.jpeg)

---

**Implementation Using Google Charts Library for React**

- Sales Chart

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

- Weekly Sales Pie Chart

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

**Explanation:**

- **Sales Chart**: Displays detailed sales data in a table format using the `Chart` component from Google Charts for React. It includes options for pagination, styling with custom CSS classes (`chart-row`, `chart-header-row`, `chart-cell`), and a loading indicator (`loader`) while data is fetched.
- **Weekly Sales Pie Chart**: Shows a pie chart visualization of weekly sales data using the `PieChart` component. It includes a loading indicator (`loader`) and options for chart title (`Weekly Sales`) and slice visibility threshold.

**Usage:**
These components are integrated into the React web client to provide administrators with visual representations of daily and weekly sales data, enhancing decision-making and monitoring capabilities.

---

# Application Walkthrough :tv:

[![Application Walkthrough](http://img.youtube.com/vi/MgLPjGejU7s/0.jpg)](https://youtu.be/MgLPjGejU7s)
