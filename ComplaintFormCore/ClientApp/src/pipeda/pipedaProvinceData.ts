import { PipedaProvince } from "./pipedaProvince";
import { Province } from "../surveyHelper";

export class PipedaProvinceData {
    public FrenchPrefix_Au = "";
    public FrenchPrefix_Du = "";
    public Province_link = "";
    public Link_province_opc = "";
    public Link_more_info = "";
}

export const PipedaProvincesData: Record<Province, PipedaProvince> = {
    [Province.Ontario]: {
        French: {
            FrenchPrefix_Au: "en ",
            FrenchPrefix_Du: "de l'",
            Link_more_info: "",
            Link_province_opc: "https://www.ipc.on.ca/protection-de-la-vie-privee-particuliers/proteger-sa-vie-privee-2/?lang=fr",
            Province_link: ""
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "",
            Link_province_opc: "https://www.ipc.on.ca/privacy/filing-a-privacy-complaint/",
            Province_link: ""
        }
    },
    [Province.Quebec]: {
        French: {
            FrenchPrefix_Au: "au ",
            FrenchPrefix_Du: "du ",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: "https://www.cai.gouv.qc.ca/"
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: "https://www.cai.gouv.qc.ca/english/"
        }
    },
    [Province.NovaScotia]: {
        French: {
            FrenchPrefix_Au: "en ",
            FrenchPrefix_Du: "de la ",
            Link_more_info: "",
            Link_province_opc: "https://foipop.ns.ca/publictools",
            Province_link: ""
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "",
            Link_province_opc: "https://foipop.ns.ca/publictools",
            Province_link: ""
        }
    },
    [Province.NewBrunswick]: {
        French: {
            FrenchPrefix_Au: "au ",
            FrenchPrefix_Du: "du ",
            Link_more_info: "",
            Link_province_opc: "https://oic-bci.ca/?lang=fr",
            Province_link: ""
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "",
            Link_province_opc: "http://www.beta-theta.com/information-and-privacy.html",
            Province_link: ""
        }
    },
    [Province.Manitoba]: {
        French: {
            FrenchPrefix_Au: "au ",
            FrenchPrefix_Du: "du ",
            Link_more_info: "https://www.ombudsman.mb.ca/info/access-and-privacy-fr.html",
            Link_province_opc: "",
            Province_link: ""
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "https://www.ombudsman.mb.ca/info/access-and-privacy-division.html",
            Link_province_opc: "",
            Province_link: ""
        }
    },
    [Province.BritishColumbia]: {
        French: {
            FrenchPrefix_Au: "en ",
            FrenchPrefix_Du: "de la ",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: "https://www.oipc.bc.ca/for-the-public/"
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: "https://www.oipc.bc.ca/for-the-public/"
        }
    },
    [Province.PEI]: {
        French: {
            FrenchPrefix_Au: "à l'",
            FrenchPrefix_Du: "de l'",
            Link_more_info: "https://www.assembly.pe.ca/",
            Link_province_opc: "",
            Province_link: ""
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "https://www.assembly.pe.ca/",
            Link_province_opc: "",
            Province_link: ""
        }
    },
    [Province.Saskatchewan]: {
        French: {
            FrenchPrefix_Au: "en ",
            FrenchPrefix_Du: "de la ",
            Link_more_info: "https://oipc.sk.ca/",
            Link_province_opc: "",
            Province_link: ""
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "https://oipc.sk.ca/",
            Link_province_opc: "",
            Province_link: ""
        }
    },
    [Province.Alberta]: {
        French: {
            FrenchPrefix_Au: "en ",
            FrenchPrefix_Du: "de l'",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: "https://www.oipc.ab.ca/action-items/request-a-review-file-a-complaint.aspx"
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: "https://www.oipc.ab.ca/action-items/request-a-review-file-a-complaint.aspx"
        }
    },
    [Province.Newfoundland]: {
        French: {
            FrenchPrefix_Au: "à ",
            FrenchPrefix_Du: "de ",
            Link_more_info: "",
            Link_province_opc: "https://www.oipc.nl.ca/public/investigations/privacy",
            Province_link: ""
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "",
            Link_province_opc: "https://www.oipc.nl.ca/public/investigations/privacy",
            Province_link: ""
        }
    },
    [Province.Nunavut]: {
        French: {
            FrenchPrefix_Au: "au ",
            FrenchPrefix_Du: "du ",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: ""
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: ""
        }
    },
    [Province.Yukon]: {
        French: {
            FrenchPrefix_Au: "au ",
            FrenchPrefix_Du: "du ",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: ""
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: ""
        }
    },
    [Province.NWT]: {
        French: {
            FrenchPrefix_Au: "aux ",
            FrenchPrefix_Du: "des ",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: ""
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: ""
        }
    },
    [Province.Other]: {
        French: {
            FrenchPrefix_Au: "à l'",
            FrenchPrefix_Du: "de l'",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: ""
        },
        English: {
            FrenchPrefix_Au: "",
            FrenchPrefix_Du: "",
            Link_more_info: "",
            Link_province_opc: "",
            Province_link: ""
        }
    }
};
