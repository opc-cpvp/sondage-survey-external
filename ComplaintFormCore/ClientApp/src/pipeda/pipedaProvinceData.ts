import { PipedaProvince } from "./pipedaProvince";
import { Province } from "../surveyHelper";

export interface PipedaProvinceData {
    FrenchPrefix_Au: string;
    FrenchPrefix_Du: string;
    Province_link: string;
    Link_province_opc: string;
    Link_more_info: string;
}

export const PipedaProvincesData: Map<Province, PipedaProvince> = new Map([
    [
        Province.Ontario,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "",
                Link_province_opc: "https://www.ipc.on.ca/privacy/filing-a-privacy-complaint/",
                Province_link: ""
            },
            French: {
                FrenchPrefix_Au: "en ",
                FrenchPrefix_Du: "de l’",
                Link_more_info: "",
                Link_province_opc: "https://www.ipc.on.ca/protection-de-la-vie-privee-particuliers/proteger-sa-vie-privee-2/?lang=fr",
                Province_link: ""
            }
        }
    ],
    [
        Province.Quebec,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: "https://www.cai.gouv.qc.ca/english/"
            },
            French: {
                FrenchPrefix_Au: "au ",
                FrenchPrefix_Du: "du ",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: "https://www.cai.gouv.qc.ca/"
            }
        }
    ],
    [
        Province.NovaScotia,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "",
                Link_province_opc: "https://foipop.ns.ca/publictools",
                Province_link: ""
            },
            French: {
                FrenchPrefix_Au: "en ",
                FrenchPrefix_Du: "de la ",
                Link_more_info: "",
                Link_province_opc: "https://foipop.ns.ca/publictools",
                Province_link: ""
            }
        }
    ],
    [
        Province.NewBrunswick,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "",
                Link_province_opc: "http://www.beta-theta.com/information-and-privacy.html",
                Province_link: ""
            },
            French: {
                FrenchPrefix_Au: "au ",
                FrenchPrefix_Du: "du ",
                Link_more_info: "",
                Link_province_opc: "https://oic-bci.ca/?lang=fr",
                Province_link: ""
            }
        }
    ],
    [
        Province.Manitoba,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "https://www.ombudsman.mb.ca/info/access-and-privacy-division.html",
                Link_province_opc: "",
                Province_link: ""
            },
            French: {
                FrenchPrefix_Au: "au ",
                FrenchPrefix_Du: "du ",
                Link_more_info: "https://www.ombudsman.mb.ca/info/access-and-privacy-fr.html",
                Link_province_opc: "",
                Province_link: ""
            }
        }
    ],
    [
        Province.BritishColumbia,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: "https://www.oipc.bc.ca/for-the-public/"
            },
            French: {
                FrenchPrefix_Au: "en ",
                FrenchPrefix_Du: "de la ",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: "https://www.oipc.bc.ca/for-the-public/"
            }
        }
    ],
    [
        Province.PEI,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "https://www.assembly.pe.ca/",
                Link_province_opc: "",
                Province_link: ""
            },
            French: {
                FrenchPrefix_Au: "à l’",
                FrenchPrefix_Du: "de l’",
                Link_more_info: "https://www.assembly.pe.ca/",
                Link_province_opc: "",
                Province_link: ""
            }
        }
    ],
    [
        Province.Saskatchewan,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "https://oipc.sk.ca/",
                Link_province_opc: "",
                Province_link: ""
            },
            French: {
                FrenchPrefix_Au: "en ",
                FrenchPrefix_Du: "de la ",
                Link_more_info: "https://oipc.sk.ca/",
                Link_province_opc: "",
                Province_link: ""
            }
        }
    ],
    [
        Province.Alberta,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: "https://www.oipc.ab.ca/action-items/request-a-review-file-a-complaint.aspx"
            },
            French: {
                FrenchPrefix_Au: "en ",
                FrenchPrefix_Du: "de l’",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: "https://www.oipc.ab.ca/action-items/request-a-review-file-a-complaint.aspx"
            }
        }
    ],
    [
        Province.Newfoundland,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "",
                Link_province_opc: "https://www.oipc.nl.ca/public/investigations/privacy",
                Province_link: ""
            },
            French: {
                FrenchPrefix_Au: "à ",
                FrenchPrefix_Du: "de ",
                Link_more_info: "",
                Link_province_opc: "https://www.oipc.nl.ca/public/investigations/privacy",
                Province_link: ""
            }
        }
    ],
    [
        Province.Nunavut,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: ""
            },
            French: {
                FrenchPrefix_Au: "au ",
                FrenchPrefix_Du: "du ",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: ""
            }
        }
    ],
    [
        Province.Yukon,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: ""
            },
            French: {
                FrenchPrefix_Au: "au ",
                FrenchPrefix_Du: "du ",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: ""
            }
        }
    ],
    [
        Province.NWT,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: ""
            },
            French: {
                FrenchPrefix_Au: "aux ",
                FrenchPrefix_Du: "des ",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: ""
            }
        }
    ],
    [
        Province.Other,
        {
            English: {
                FrenchPrefix_Au: "",
                FrenchPrefix_Du: "",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: ""
            },
            French: {
                FrenchPrefix_Au: "à l’",
                FrenchPrefix_Du: "de l’",
                Link_more_info: "",
                Link_province_opc: "",
                Province_link: ""
            }
        }
    ]
]);
