import { PipedaProvince } from "./pipedaProvince";

export class PipedaProvinceData {
    public FrenchPrefix_Au = "";
    public FrenchPrefix_Du = "";
    public Province_link = "";
    public Link_province_opc = "";
    public Link_more_info = "";
}

export const PipedaProvincesData: Record<number, PipedaProvince> = {
    1: {
        // Ontario
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
    2: {
        // Quebec
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
    3: {
        // NovaScotia
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
    4: {
        // NewBrunswick
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
    5: {
        // Manitoba
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
    6: {
        // British Columbia
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
    7: {
        // PEI
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
    8: {
        //  Saskatchewan
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
    9: {
        //  Alberta
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
    10: {
        //  Newfoundland
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
    11: {
        //  Nunavut
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
    12: {
        //  Yukon
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
    13: {
        //  NWT
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
    14: {
        //  Other
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
